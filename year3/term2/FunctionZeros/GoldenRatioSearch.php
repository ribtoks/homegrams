<?php

define('PHI_LONGER', 0.618033989); // (sqrt(5) - 1)/2
define('PHI_SHORTER', 0.381966011); // (3 - sqrt(5))/2

function GoldenRatioSearch($function, $a, $b, $epsilon, $stable=false, $verbose=false)
{
  $left = $a;
  $right = $b;

  if ($verbose)
    echo "[$left, $right]<br />";

  // short preinitialization of
  // iteration process
  $xCurr = $left + PHI_SHORTER*($right - $left);
  $xPrev = ($right + $left) - $xCurr;
  $funcPrev = $function($xPrev);
  $funcCurr = $function($xCurr);
  
  if ($funcCurr <= $funcPrev)
    {
      $right = $xPrev;
      $xPrev = $xCurr;
      $funcPrev = $funcCurr;
    }
  else
      $left = $xCurr;

  if ($verbose)
    echo "[$left, $right]<br />";

  // one iteration is already done
  $iterationsCount = 1;

  // main loop though segment
  while (abs($left - $right) >= $epsilon)
    {
      if ($stable)
	{
	  $tempDiff = $right - $left;
	  $xTemp1 = $left + PHI_SHORTER*$tempDiff;
	  $xTemp2 = $left + PHI_LONGER*$tempDiff;

	  if (abs($xTemp1 - $xPrev) > abs($xTemp2 - $xPrev))
	    $xCurr = $xTemp1;
	  else
	    $xCurr = $xTemp2;
	}
      else
	$xCurr = $left + $right - $xPrev;

      $funcCurr = $function($xCurr);

      $x1 = $xPrev;
      $x2 = $xCurr;
      
      $f1 = $funcPrev;
      $f2 = $funcCurr;
      if ($xPrev > $xCurr)
	{
	  // swap values
	  list($x1, $x2) = array($x2, $x1);
	  list($f1, $f2) = array($f2, $f1);
	}

      if ($f1 <= $f2)
	{
	  $right = $x2;
	  $xPrev = $x1;
	  $funcPrev = $f1;
	}
      else
	{
	  $left = $x1;
	  $xPrev = $x2;
	  $funcPrev = $f2;
	}
      
      ++$iterationsCount;
      
      if ($verbose)
	echo "[$left, $right]<br />";
    }

  $x = min($xPrev, $xCurr);

  if ($verbose)
    echo "<br />Iterations count: $iterationsCount<br /><br />";

  //  if (abs($function($x)) < $epsilon)
  return $x;
  //return false;
}

?>
