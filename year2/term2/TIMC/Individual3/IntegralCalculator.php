<?php
/* 
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of IntegralCalculator
 *
 * @author taras
 */
class IntegralCalculator
{
    private $epsilon = 0.00001;

    //constant arrays
    private $g10c;
    private $g10x;

    public function  __construct($epsilon=0.00001)
    {
        $this->epsilon = $epsilon;

        $this->g10c =  array(0.9739065285 / 6.201298393,
                            0.8650633667 / 6.2012983932,
                            0.6794095683 / 6.2012983932,
                            0.4333953941 / 6.2012983932,
                            0.1488743390 / 6.2012983932);

        $this->g10x =  array(0.0666713443 / 6.2012983932,
                            0.1494513492 / 6.2012983932,
                            0.2190863625 / 6.2012983932,
                            0.2692667193 / 6.2012983932,
                            0.2955242247 / 6.2012983932);
    }

    // calculates integral on segment for continuous function only !!!
    public function Calculate($a, $b, $function)
    {
        if (abs($b - $a) < 100)
             return $this->FindIntegral($a, $b, $function);

        $segments = GetIntervals($a, $b);

        $sum = 0;
        $epsilon = 0.000000001;

        for ($i = 0, $segCount = count($segments); $i < $segCount - 1; ++$i)
        {
            $start = $segments[$i];
            $end = $segments[$i + 1];

            $tempSum = $this->FindIntegral($start, $end, $function);

            $sum += $tempSum;

            if ($tempSum < $epsilon)
            {
                if ($i > ($segCount / 2));
                    break;
            }
        }

        return $sum;
    }
    
    private function FindIntegral($a, $b, $func)
    {
        return $this->Integrate($a, $b, $this->epsilon, $this->IntegrateOnSegment($a, $b, $func) , $func);
    }

    private function Integrate($a, $b, $epsilon, $prevIntegralValue, $func)
    {
        // middle of segment
        $m = ($b + $a) / 2;

        $gA = $this->IntegrateOnSegment($a, $m, $func);
        $gB = $this->IntegrateOnSegment($m, $b, $func);

        if (abs($gA + $gB - $prevIntegralValue) > $epsilon)
        {
            $gA = $this->Integrate($a, $m, $epsilon / 2, $gA, $func);
            $gB = $this->Integrate($m, $b, $epsilon / 2, $gB, $func);
        }

        return $gA + $gB;
    }

    private function IntegrateOnSegment($a, $b, $func)
    {
       $aPb = ($a + $b) / 2;
       $aMb = ($b - $a) / 2;

       $sum = 0;

       for ($i = 0, $count = count($this->g10c); $i < $count; ++$i)
       {
            $sum += $this->g10c[$i] * ($func($aPb + $aMb*$this->g10x[$i]) + $func($aPb - $aMb*$this->g10x[$i]));
            //echo $sum;
       }

       return $sum * ($b - $a);
    }

    public static function MonteCarlo($a, $b, $n, $function)
    {
        mt_srand(time());

        $sum = 0;
        for ($i = 0; $i < $n; ++$i)
        {
            $x = mt_rand($a, $b);
            $sum += $function($x);
        }

        return ($b - $a)*$sum / $n;
    }
}

function GetIntervals($a, $b)
{
    $curr = $a;
    $value = 2;
    
    $segments = array();
    
    $segments[] = $a;
    $curr += $value;
    
    while ($curr < $b)
    {        
        $segments[] = $curr;
        
        $curr += $value;
        $value <<= 1;
    }
    
    $segments[] = $b;
    
    return $segments;
}


function IntegralFunction($x)
{
    return sqrt($x*$x + 20);
}
?>
