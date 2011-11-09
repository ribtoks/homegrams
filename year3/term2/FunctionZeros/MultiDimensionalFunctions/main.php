<?php

require_once 'FindMinimum.php';
require_once 'VectorsOperations.php';

function UserFunc($x)
{
   $x1 = $x[0];
   $x2 = $x[1];
   
   $x1_s = $x1*$x1;
   $x2_s = $x2*$x2;
   
   return sqrt($x1_s + $x2_s + 2) + 2*$x1 + 3*$x2 + $x1_s*$x1_s + $x2_s*$x2_s;
}

function UserFuncDerivative($x1, $x2)
{
   $x1_s = $x1*$x1;
   $x2_s = $x2*$x2;

   $sqrt = sqrt($x1_s + $x2_s + 2);

   return array(
      ($x1/$sqrt) + 2 + 4*pow($x1, 3),
      ($x2/$sqrt) + 3 + 4*pow($x2, 3));
}

function UserFuncDerivative2($x1, $x2)
{
   $x1_s = $x1*$x1;
   $x2_s = $x2*$x2;

   $sqrt = sqrt($x1_s + $x2_s + 2);

   $df_x1x2 = -$x1*$x2/pow($sqrt, 3);

   $arr1 = array(
      1/$sqrt - ($x1_s/pow($sqrt, 3)) + 12*$x1_s,
      $df_x1x2);

   $arr2 = array(
      $df_x1x2,
      1/$sqrt - ($x2_s/pow($sqrt, 3)) + 12*$x2_s);

   return array($arr1, $arr2);
}

function HFunc($x, $func, $func_deriv, $func_deriv2)
{
   $matr = $func_deriv2($x[0], $x[1]);
   $matrix = GetInvertibleMatrix($matr);

   $grad = $func_deriv($x[0], $x[1]);

   $matr = MulMatrixOnVector($matrix, $grad);

   $height = count($matr);

   for ($i = 0; $i < $height; ++$i)
	 $matr[$i] *= -1;

   return $matr;
}

$x0 = array(0, 0);
$epsilon = 0.001;
$x = FindMinimum('UserFunc', $x0, 1, 'GetAlpha1', 'HFunc', 
	       $epsilon, 'UserFuncDerivative', 'UserFuncDerivative2');
print_r($x);
echo "\r\n";

echo "\r\n";
echo UserFunc($x);
echo "\r\n";
?>