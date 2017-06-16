2<?php

require_once 'SegmentLocalization.php';
require_once 'BinarySearch.php';
require_once 'GoldenRatioSearch.php';
require_once 'TangentsMethodSearch.php';

function UserFunc($x)
{
   return $x*$x + 3*$x*(log($x) - 1);
}

function UserFuncDerivative($x)
{
   return 2*$x + 3*log($x);
}

//echo "BeforeGetSegment<br />";
//$segment = array('a' => 0.001,'b' => 10);//
$segment = GetSegment(UserFunc, 0.001, 20, 0.005);


//echo "AfterGetSegment";
$epsilon = 0.00001;
$delta = 0.0000001;

$xBinary = BinarySearch(UserFunc, $segment['a'], $segment['b'], $epsilon, $delta, true);

echo "x=$xBinary  F(x)=".UserFunc($xBinary);

echo "<br /><br />";

$xGolden = GoldenRatioSearch(UserFunc, $segment['a'], $segment['b'], $epsilon, false, true);
echo "x=$xGolden  F(x)=".UserFunc($xGolden);
echo "<br /><br />";

$xTangents = TangentsSearch(UserFunc, UserFuncDerivative, 2.3, $segment['a'], $segment['b'], $epsilon, true);
echo "x=$xTangents F(x)=".UserFunc($xTangents);

?>
