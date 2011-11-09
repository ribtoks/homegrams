<?php

function GetSegment($function, $left, $right, $epsilon)
{
  $step = $epsilon;  
  $xCurr = $left;

  // if on this segment function grows, that 
  // we should move backwards
  if ($function($xCurr + $step) > $function($xCurr))
    $step *= -1;

  // find where is minimum point
  while ($function($xCurr + $step) < $function($xCurr))
    {
      $xCurr += $step;
      $step *= 2;

      if ($xCurr > $right)
	 break;
    }

  // now we have minimum in the last segmen

  $Xm = $xCurr + $step;
  $Xm_1 = $xCurr;
  $Xm_2 = $$xCurr - $step;

  $step /= 2;

  $Xmp1 = $Xm - $step;

  // now we get [ $Xm_2 < $Xm_1 < $Xmp1 < $Xm ]
  
  if (abs($Xm_2 - $Xmp1) < abs($Xm_1 - $Xm))
    return array('a' => max($Xm_2, $left), 'b' => min($Xmp1, $right));
  else
    return array('a' => max($Xm_1, $left), 'b' => min($Xm, $right));
}

?>