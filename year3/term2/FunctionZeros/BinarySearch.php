<?php

function BinarySearch($function, $a, $b, $epsilon, $delta, $verbose=false)
{
  $left = $a;
  $right = $b;

  if ($verbose)
    echo "[$left, $right]<br />";

  $iterationsCount = 0;

  while (abs($left - $right) >= $epsilon)
    {
      $tempSum = $left + $right;
      
      // calculate points near center of segment
      $x1 = ($tempSum - $delta) / 2;
      $x2 = ($tempSum + $delta) / 2;

      if (($x2 >= $right) or ($x1 <= $left))
	break;

      // calculate function values in that points
      $f1 = $function($x1);
      $f2 = $function($x2);

      ++$iterationsCount;

      if ($f1 <= $f2)
	  $right = $x2;
      else
	  $left = $x1;

      if ($verbose)
	echo "[$left, $right]<br />";
    }
  
  // our solution
  $x = ($x1 + $x2) / 2;

  if ($verbose)
    echo "<br />Iterations count: $iterationsCount<br /><br />";

  //  if (abs($function($x)) < $epsilon)
  return $x;
  //  return false;
}

?>
