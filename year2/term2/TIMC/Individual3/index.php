<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title></title>
    </head>
    <body>
        <?php

        require_once 'IntegralSolver.php';

        $alpha = 0.05;//$_REQUEST['alpha'];

        $tableValues = array
        (
            array(7.95, 6.4, 5.65, 8.15, 7.1, 7.95),
            array(6.45, 5.35, 7.55, 7.60, 8.25, 7.25),
            array(5.8, 7.30, 6.05, 7.55, 7.25, 7.65),
            array(6.08, 7.15, 7.05, 7.15, 7.55, 6.05),
            array(6.85, 8.1, 7.0, 8.05, 6.25, 6.8),
            array(8.05, 7.95, 7.70, 5.65, 6.95, 8.05)
        );

        $tableNames = array
        (
            array('A', 'B', 'C', 'D', 'E', 'F'),
            array('B', 'C', 'F', 'A', 'D', 'E'),
            array('C', 'F', 'B', 'E', 'A', 'D'),
            array('D', 'A', 'E', 'B', 'F', 'C'),
            array('E', 'D', 'A', 'F', 'C', 'B'),
            array('F', 'E', 'D', 'C', 'B', 'A')
        );

        $m = 6;

        $generalMean = 0;
        for ($i = 0; $i < $m; ++$i)
            for ($j = 0; $j < $m; ++$j)
                $generalMean += $tableValues[$i][$j];
        $generalMean /= $m*$m;


        $means = array();
        $means['A'] = 0;
        $means['B'] = 0;
        $means['C'] = 0;
        $means['D'] = 0;
        $means['E'] = 0;
        $means['F'] = 0;

        for ($i = 0; $i < $m; ++$i)
            for ($j = 0; $j < $m; ++$j)
                $means[ $tableNames[$i][$j] ] += $tableValues[$i][$j];

        foreach ($means as $key => &$value)
            $value /= $m;

        // calculate row means
        $rowMeans = array();
        for ($i = 0; $i < $m; ++$i)
            $rowMeans[] = array_sum($tableValues[$i]) / $m;

        // calculate column means
        $columnMeans = array();
        for ($j = 0; $j < $m; ++$j)
        {
            $columnMeans[$j] = 0;
            for ($i = 0; $i < $m; ++$i)
                $columnMeans[$j] += $tableValues[$i][$j];
            $columnMeans[$j] /= $m;
        }
        
        $integralSolver = new IntegralSolver($m - 1, ($m - 1)*($m - 2));

        $upperCritical = $integralSolver->SolvePhisherUpper($alpha / 2);
        $lowerCritical = $integralSolver->SolvePhisherLower($alpha / 2);

        echo "Interval - [$lowerCritical, $upperCritical] <br /><br />";
        // calculate dependencies...

        $empiricNames = 0;
        foreach ($means as $key => $value)
        {
            $temp = ($value - $generalMean);
            $empiricNames += $temp*$temp;
        }
        $empiricNames *= $m / ($m - 1);


        $empiricRows = 0;
        foreach ($rowMeans as $x)
        {
            $temp = ($x - $generalMean);
            $empiricRows += $temp*$temp;
        }
        $empiricRows *= $m / ($m - 1);


        $empiricColumns = 0;
        foreach ($columnMeans as $x)
        {
            $temp = ($x - $generalMean);
            $empiricColumns += $temp*$temp;
        }
        $empiricColumns *= $m / ($m - 1);


        // calculate remaining variance

        $kArray = array_values($means);

        $remaining = 0;
        for ($i = 0; $i < $m; ++$i)
        {
            for ($j = 0; $j < $m; ++$j)
            {
                $temp = $tableValues[$i][$j] - $rowMeans[$i] - $columnMeans[$j] - $kArray[$i] + 2*$generalMean;
                $remaining += $temp*$temp;
            }
        }
        $remaining /= ($m - 1)*($m - 2);


        $empiricNames /= $remaining;
        $empiricRows /= $remaining;
        $empiricColumns /= $remaining;


        function IsIn($a, $b, $x)
        {
            return ($x >= $a) && ($x <= $b);
        }

        //if (IsIn($lowerCritical, $upperCritical, $empiricNames))
                echo "Names provide influence! - $empiricNames<br />";

        //if (IsIn($lowerCritical, $upperCritical, $empiricRows))
                echo "Rows provide influence! - $empiricRows<br />";

        //if (IsIn($lowerCritical, $upperCritical, $empiricColumns))
                echo "Columns provide influence! - $empiricColumns<br />";

        ?>
    </body>
</html>
