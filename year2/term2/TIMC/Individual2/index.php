<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>Chi-Square check</title>
    </head>
    <body>
        <div style="margin-left:40px;">
        <?php
        require_once 'Classes/ChiSquareChecker.php';
        require_once 'Classes/Functions.php';

        //if (!isset($_REQUEST['textbox']))
        //    die("Input parameters are empty. Textbox is empty.");
    
        //$a = "[300, 400] 1\r\n[400, 500] 9\r\n[500, 600] 18\r\n[600, 700] 33\r\n[700, 800] 40\r\n[800, 900] 52\r\n[900, 1000] 29\r\n[1000, 1100] 14\r\n[1100, 1200] 4";

        //$a = "0 112\r\n1 168\r\n2 130\r\n3 69\r\n4 32\r\n5 5\r\n6 1\r\n7 1";

        // all lines with data
        $lines = explode("\r\n", $_REQUEST['textbox']);

        $sampling = array();
        $frequencies = array();

        $isContinuous = $_REQUEST['IsContinuous'];
        $alpha = $_REQUEST['alpha'];
        $epsilon = $_REQUEST['epsilon'];

        foreach($lines as $line)
        {
            if ($isContinuous)
            {
                $a = 0;
                $b = 0;
                $count = 0;

                sscanf($line, '[%f, %f] %d', $a, $b, $count);

                if ($a >= $b)
                    die("Left bound of interval is not less, that right bound.");

                $value = ($a + $b) / 2;
                $segments[] = array('a' => $a, 'b' => $b);
            }
            else
            {
                $value = 0;
                $count = 0;

                sscanf($line, '%f %d', $value, $count);
            }

            $sampling[] = $value;
            $frequencies[] = $count;
        }

        echo "Parsed sampling:<br />";
        HTMLTable::start();
        HTMLTable::printRow("Values", $sampling);
        HTMLTable::printRow("Frequencies", $frequencies);
        HTMLTable::end();

        $samplingCount = count($sampling);

        // Select function of distribution
        echo "<br /><br />";

        echo '<span style="font-weight:bold; font-size:14px; margin-left:20px;
            border-width:1px; border-style:solid; border-color:black; padding:5px;
            background-color:yellow;">';
        echo "Check the hypotesis, that current sampling has ";

        if ($_REQUEST['Distribution'] == 1)
        {
            // Poisson Distribution

            echo '<font style="text-transform: uppercase;">Poisson';

            $sum = 0;

            foreach(array_combine($sampling, $frequencies) as $key => $value)
                $sum += $key * $value;

            // $lambda is mean of sampling
            $lambda = $sum / array_sum($frequencies);

            // function of Poisson distribution
            $func = create_function('$a', 'return pow('.$lambda.', $a) /
                                                (exp('.$lambda.') * Factorial($a));');
            /*
            $func = function ($a) use($lambda)
            {
                return pow($lambda, $a) / (exp($lambda) * Factorial($a));
            };
            */

            $appreciatedArgsCount = 1;
        }
        elseif ($_REQUEST['Distribution'] == 2)
        {
            // Normal Distribution

            echo '<font style="text-transform: uppercase;">Normal';

            $sum = 0;

            foreach(array_combine($sampling, $frequencies) as $key => $value)
                $sum += $key * $value;

            $n = array_sum($frequencies);

            // $lambda is mean of sampling
            $a = $sum / $n;

            //$code = '$temp = $newArg - '.$a.';
            //    $prevSum += $temp*$temp;
            //    return $prevSum;';
            //$tempFunc = create_function('$prevSum, $newArg', $code);
            /*
            $tempFunc = function($prevSum, $newArg) use ($a)
            {
                $temp = $newArg - $a;
                $prevSum += $temp*$temp;
                return $prevSum;
            };
            
            $sigmaSquare = array_reduce($sampling, $tempFunc, 0) / $n;
            */

            $sigmaSquare = 0;
            foreach (array_combine($sampling, $frequencies) as $key => $value)
                $sigmaSquare += ($key - $a) * ($key - $a) * $value;

            $sigmaSquare /= $n;

            /*for ($i = 0, $count = count($sampling); $i < $count; ++$i)
            {
                $temp = $sampling[$i] - $a;
                $sigmaSquare += $temp*$temp;
            }

            $sigmaSquare /= $n;
            */

            $constant1 = sqrt(2*pi()*$sigmaSquare);
            $constant2 = 2*$sigmaSquare;

            $code = '$numerator = $b - '.$a.';';
            $code .= 'return exp(-$numerator*$numerator / '.$constant2.');';

            /*
            $expFunc = function($b) use ($a, $constant2)
            {
                $numerator = $b - $a;
                return exp(-$numerator*$numerator / $constant2);
            };
            */
            $expFunc = create_function('$b', $code);

            /*
            $func = function($x, $a, $b) use($constant1, $expFunc)
            {
                $intCalc = new IntegralCalculator();
                return $intCalc->Calculate($a, $b, $expFunc) / $constant1;
            };
            */
            $code = 'global $expFunc, $epsilon;';
            $code .= '$intCalc = new IntegralCalculator($epsilon);';
            $code .= 'return $intCalc->Calculate($a, $b, $expFunc) / '.$constant1.';';

            $func = create_function('$x, $a, $b', $code);
            $appreciatedArgsCount = 2;
        }
        /*
        elseif ($_REQUEST['Distribution'] == 3)
        {
            // Continuous Distribution

            echo '<font style="text-transform: uppercase;">Continuous';

            $length = 1200 - 400;

            $func = function($x) use ($length)
            {
                return 1 / $length;
            };

            $appreciatedArgsCount = 0;
        }
        */
        echo " Distribution</font></span><br /><br /><br />";

        $debug = true;

        $chiSquareChecker = new ChiSquareChecker($sampling, $frequencies,
                $func, $appreciatedArgsCount, $debug, $epsilon,
                $isContinuous, $segments);

        $result = $chiSquareChecker->IsSamplingOk($alpha);

        if (!$debug)
        {
            if ($result)
                echo "Sampling is ok.";
            else
                echo "Bad sampling.";
        }
        ?>
        </div>
    </body>
</html>
