<?php

require_once 'BinarySearch.php';

function TangentsSearch($function, $func_derivative, $L, $a, $b, $epsilon, $verbose=false)
{
   $x0 = ($a + $b)/2 + ($function($a) - $function($b))/(2*$L);


   $tangentFunc = function($x, $xDiff) use($function, $func_derivative)
   {
      return $function($xDiff) + $func_derivative($xDiff)*($x - $xDiff);
   };


   $qFunc = function($x) use($tangentFunc, $x0)
   {
      return $tangentFunc($x, $x0);
   };

   $x1 = BinarySearch($qFunc, $a, $b, $epsilon, $epsilon / 100);

   $funcDifference = function($func, $x1, $x2)
   {
      return $func($x1) - $func($x2);
   };

   $solveFunc = function($t, $s) 
      use ($function, $func_derivative, $funcDifference)
   {
      $fDiff = $funcDifference($function, $s, $t);
      $fDerivDiff = $funcDifference($func_derivative, $t, $s);

      return ( $fDiff + ($t*$func_derivative($t) -
			   $s*$func_derivative($s)) ) / $fDerivDiff;
   };

   $currLeftTangent = $x0;
   $currRightTangent = $x1;
   $crossPoint = $solveFunc($currLeftTangent, $currRightTangent);

   if ($tangentFunc($x0, $crossPoint) <
       $tangentFunc($x1, $crossPoint))
      $currLeftTangent = $x0;
   else
      $currLeftTangent = $x1;

   $currRightTangent = $crossPoint;

   if ($verbose)
      echo "[$currLeftTangent, $currRightTangent]<br /><br />";

   $crossPoint = $solveFunc($currLeftTangent, $currRightTangent);
 

   $iterationsCount = 0;
   
   while (abs($currLeftTangent - $currRightTangent) > $epsilon)
   {
      $left = $solveFunc($currLeftTangent, $crossPoint);
      $right = $solveFunc($currRightTangent, $crossPoint);

      if ($tangentFunc($left, $crossPoint) <
	  $tangentFunc($right, $crossPoint))
	 $currLeftTangent = $currLeftTangent;
      else
	 $currLeftTangent = $currRightTangent;

      $currRightTangent = $crossPoint;

      if ($verbose)
	 echo "[$currLeftTangent, $currRightTangent]<br />";

      $crossPoint = $solveFunc($currLeftTangent, $currRightTangent);

      ++$iterationsCount;
   }

   echo "<br />Iterations Count: $iterationsCount<br /><br />";

   return $crossPoint;
}

?>