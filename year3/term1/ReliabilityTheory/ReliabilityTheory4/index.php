<?php
include_once 'data.php';
require_once 'DiscretePlotter.php';
require_once 'ContinuousPlotter.php';
require_once 'GammaFunc.php';
require_once 'HTMLTable.php';
     
global $AvailableColors;
global $values;
global $states;


$arr = array_combine($values, $states);
ksort($arr);
$values = array_keys($arr);
$states = array_values($arr);


function Quantile($alpha, $arr, $arrCount)
{
    if ($alpha <= 0 Or $alpha >= 100)
        return null;

    $index = ($alpha * $arrCount) / 100;

    if (floor($index) == $index)
        return $arr[$index - 1];

    return null;
}

function GetQuantile($alpha, $delta, $arr, $arrCount)
{
    $q1 = Quantile($alpha - $delta, $arr, $arrCount);
    $q2 = Quantile($alpha + $delta, $arr, $arrCount);

    if (!is_null($q1))
        return $q1;

    if (!is_null($q2))
        return $q2;

    return GetQuantile($alpha, $delta + 1, $arr, $arrCount);
}

function GetStartNju()
{
    global $values;

    $p1 = 17;
    $p2 = 97;

    $p1P = $p1 / 100;
    $p2P = $p2 / 100;

    $count = count($values);

    $tp1 = GetQuantile($p1, 0, $values, $count);
    $tp2 = GetQuantile($p2, 0, $values, $count);

    return ( log(-log(1 - $p1P)) - log(-log(1 - $p2P)) ) / (log($tp1) - log($tp2));
}

function GetNju($startNju)
{
    global $values;
    global $states;

    $arr = array_combine($values, $states);
    $filterFunc = function($x)
    {
        // == F
        return ($x == 0);
    };

    $lnFunc = function($x)
    {
        return log($x);
    };

    $fails = array_filter($arr, $filterFunc);
    $fails = array_flip($fails);
    $leftSide = array_sum( array_map($lnFunc, $fails) ) / count($fails);

    //echo $leftSide;

    $epsilon = 1;//0.0000001;

    $lastNju = 0;
    $currNju = $startNju;

    while (abs($currNju - $lastNju) > $epsilon)
    {
        $greatSum = 0;
        $smallSum = 0;

        foreach ($values as $x)
        {
            $temp = pow($x, $currNju);
            $greatSum += $temp * log($x)*$currNju;
            $smallSum += $temp;
        }

        $rightSide = ($greatSum / $smallSum) - 1;
        $lastNju = $currNju;
        $currNju = $rightSide / $leftSide;

        //echo "$lastNju - $currNju<br />";
    }

    return ($lastNju + $currNju) / 2;
}

function GetSigma($nju)
{
    global $values;
    
    $power = function ($x) use($nju)
    {
        return pow($x, $nju);
    };
    
    $n = count($values);
    
    return pow((array_sum( array_map($power, $values) ) / $n), 1 / $nju);
}


$startNju = GetStartNju();
$realNju = GetNju($startNju);
$sigma = GetSigma($realNju);

//echo "$startNju<br />$realNju<br />$sigma";

$Func = function($t) use($realNju, $sigma)
{
    return 1 - exp( -(pow($t / $sigma, $realNju)) );
};

$precision = 0.001;
$njuValue = round($realNju, $precision);
$sigmaValue = round($sigma, $precision);

$valuesCount = count($values);

$minIndexFunc = function($t) use($values, $valuesCount)
{
    $i = 0;
    for (; $i < $valuesCount; ++$i)
    {
        if ($t < $values[$i])
            break;
    }

    return $i - 1;
};

$maxIndexFunc = function($t) use($values, $valuesCount)
{
    $i = $valuesCount - 1;
    for (; $i >= 0; --$i)
    {
        if ($t > $values[$i])
            break;
    }
    return $i + 1;
};

$Median = function($x) use ($values, $valuesCount, $maxIndexFunc, $minIndexFunc)
{
    $minIndex = $minIndexFunc($x);
    $maxIndex = $maxIndexFunc($x);

    //echo "$minIndex - $maxIndex<br />";

    if (abs($values[$maxIndex] - $x) < 0.01)
        $index = $maxIndex;

    if (abs($values[$minIndex] - $x) < 0.01)
        $index = $minIndex;

    if (isset ($index))
        return ($index - 0.3) / ($valuesCount + 0.4);

    $minValue = ($minIndex - 0.3) / ($valuesCount + 0.4);
    $maxValue = ($maxIndex - 0.3) / ($valuesCount + 0.4);

    //$index = (($x - $values[$minIndex]) / ($values[$maxIndex] - $values[$minIndex])) + $minIndex;

    $y = ($x - $values[$minIndex]) * (($maxValue - $minValue) / ($values[$maxIndex] - $values[$minIndex]));

    return $y + $minValue;
    //return ($index - 0.3) / ($valuesCount + 0.4);
};

$MedianSimple = function($x) use ($values, $valuesCount, $maxIndexFunc, $minIndexFunc)
{
    $minIndex = $minIndexFunc($x);
    $maxIndex = $maxIndexFunc($x);

    //echo "$minIndex - $maxIndex<br />";

    if (abs($values[$maxIndex] - $x) <= abs($values[$minIndex] - $x))
        $index = $maxIndex;
    else
        $index = $minIndex;

    return ($index - 0.3) / ($valuesCount + 0.4);
};



$funcs = array();

// ---------------------------------------------

$valuesSum = array_sum($values);

$s1 = GammaFunction::Calculate($valuesCount);
$s1 /= pow($valuesSum, $valuesCount);

$sigma = $valuesSum/$valuesCount;

$funcs[] = function ($t) use ($sigma)
{
    return exp(-$t/$sigma)*10000/$sigma;
};

// ---------------------------------------------

$nju = $realNju;

$mean = $valuesSum/$valuesCount;

$func = function ($x) use ($mean)
{
    $temp = $x - $mean;
    return $temp*$temp;
};

$variance = array_sum(array_map($func, $values)) / ($valuesCount - 1);

$nju = $mean*$mean/$variance;

$sigma = $variance/$mean;

$s2 = log(GammaFunction::Calculate($nju*$valuesCount)) +
    ($nju - 1)*log($valuesSum) - $valuesCount -
    $valuesCount*log(GammaFunction::Calculate($nju)) -
    $valuesCount*$nju*log($valuesSum);


$funcs[] = function($t) use ($nju, $sigma)
{
    $temp = 1;
    $temp /= pow($sigma, $nju);
    $temp /= GammaFunction::Calculate($nju);

    $temp /= exp($t/$sigma);
    $temp *= pow($t, $nju - 1);

    return $temp*10000;//0000000;
};

// ------------------------------------------------

$nju = $realNju;
$sigma = GetSigma($realNju);
$func = function ($x) use ($nju)
{
    return pow($x, $nju);
};

$s3 = log(GammaFunction::Calculate($valuesCount)) +
    ($valuesCount - 1)*log($nju) + ($nju - 1)*array_sum(array_map(log, $values)) +
        $valuesCount*log(array_sum(array_map($func, $values)));

$funcs[] = function ($t) use ($nju, $sigma)
{
    $temp = $nju / pow($sigma, $nju);
    $temp *= pow($t, $nju - 1)*exp(-pow($t/$sigma, $nju));
    return $temp*10000;
};

// ----------------------------------------------------

//$sMax = max($s1, $s2, $s3);


if (isset($_REQUEST['graphic']))
{
    $plotter = new ContinuousPlotter(min($values) - 1, max($values) + 1, $funcs,
            "Weibull distribution with nju=$njuValue and sigma=$sigmaValue", 'white', 
            $AvailableColors, 800, 600, 3);

    $plotter->ProcessGraphics();

    $plotter->OutGraphics(0);
    exit();
}

print_r(array($s1, $s2, $s3));

echo "<br />";

HTMLTable::start();
HTMLTable::printRow("Mean", $sigma * GammaFunction::Calculate(1 + (1 / $realNju)));
HTMLTable::printRow("Median", $sigma * ( pow(log(2), (1 / $realNju)) ) );
HTMLTable::printRow("Mode", $sigma * pow( ($realNju - 1) / $realNju, 1 / $realNju ) );

HTMLTable::printRow("Quantile25", Quantile(25, $values, $valuesCount));
HTMLTable::printRow("Quantile50", Quantile(50, $values, $valuesCount));
HTMLTable::printRow("Quantile75", Quantile(75, $values, $valuesCount));


HTMLTable::end();
?>