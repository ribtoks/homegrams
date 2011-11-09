<?php

function Add($a, $b)
{
   return $a + $b;
}

function Mul($a, $b)
{
   return $a*$b;
}

function ScalarProduct($arr1, $arr2)
{
   //return array_sum(array_map('Mul', $arr1, $arr2));
   $sum = 0;
   for ($i = 0; $i < count($arr1); ++$i)
      $sum += $arr1[$i]*$arr2[$i];

   return $sum;
}

function GetInvertibleMatrix($matr)
{
   $a = $matr[0][0];
   $b = $matr[0][1];
   $c = $matr[1][0];
   $d = $matr[1][1];
   
   $det = $a*$d - $b*$c;

   $res = array(array(), array());
   
   $res[0][0] = $d / $det;
   $res[1][1] = $a / $det;
   $res[0][1] = -$b / $det;
   $res[1][0] = -$c / $det;

   return $res;
}

function MulMatrixOnMatrix($matr1, $matr2)
{
   $res = array();

   $height1 = count($matr1);
   $width1 = count($matr1[0]);
   $height2 = count($matr2);
   $width2 = count($matr2[0]);

   for ($i = 0; $i < $height1; ++$i)
      for ($j = 0; $j < $width2; ++$j)
      {
	 $temp_sum = 0;

	 for ($k = 0; $k < $width2; ++$k)
	    $temp_sum += $matr1[$i][$k]*$matr2[$k][$j];
	 
	 $res[$i][$j] = $temp_sum;
      }

   return $res;
}

function MulMatrixOnVector($matrix, $vector)
{
   $res = array();

   for ($i = 0, $count = count($matrix); $i < $count; ++$i)
      $res[$i] = ScalarProduct($matrix[$i], $vector);

   return $res;
}

function AddVectors($vect1, $vect2)
{
   //return array_map('Add', $vect1, $vect2);
   $res = array();
   for ($i = 0; $i < count($vect1); ++$i)
      $res[$i] = $vect1[$i] + $vect2[$i];

   return $res;
}

function MulVectorOnScalar($vector, $scalar)
{
   //return array_map('Mul', $vector, array_fill(0, count($vector), $scalar));
   $res = array();
   for ($i = 0; $i < count($vector); ++$i)
      $res[$i] = $vector[$i] * $scalar;

   return $res;
}

?>