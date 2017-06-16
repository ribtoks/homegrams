<?php

require_once 'VectorsOperations.php';

function GetNext($xCurr, $alpha, $h)
{
   return AddVectors($xCurr, MulVectorOnScalar($h, $alpha));
}

function GetNextAlpha($xCurr, $function, $alpha, $h)
{
   $xNext = GetNext($xCurr, $alpha, $h);

   while ($function($xNext) >= $function($xCurr))
   {
      $alpha /= 2;
      
      $xNext = GetNext($xCurr, $alpha, $h);
   }

   // get alpha as big as possible
   while ($function($xNext) < $function($xCurr))
   {
      $alpha *= 2;

      if ($alpha > 1)
	 return $alpha / 2;
      
      $xNext = GetNext($xCurr, $alpha, $h);

      if ($function($xNext) >= $function($xCurr))
      {
	 $alpha /= 2;
	 break;
      }
   }

   return $alpha;
}

function GetAlpha1($xCurr, $function, $alpha, $h)
{
   return 1;
}

function FindMinimum($func, $x0, $alpha0, $alpha, $h, $epsilon,
		     $func_derivative=null, $func_derivative2=null)
{
   $xPrev = $x0;

   $iterations = 0;

   $h_vector = $h($xPrev, $func, $func_derivative, $func_derivative2);
   
   $currAlpha = $alpha($xPrev, $func, $alpha0, $h_vector);
   
   $xCurr = GetNext($xPrev, $currAlpha, $h_vector);
   

   while (abs($func($xCurr) - $func($xPrev)) >= $epsilon)
   {
      $xPrev = $xCurr;
      $h_vector = $h($xPrev, $func, $func_derivative, $func_derivative2);
      
      $currAlpha = $alpha($xPrev, $func, $currAlpha, $h_vector);

      $xCurr = GetNext($xPrev, $currAlpha, $h_vector);

      ++$iterations;
   }

   echo "Iterations: $iterations\r\n";

   return $xCurr;
}
?>