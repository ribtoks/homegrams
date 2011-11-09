<?php
/* 
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

class GammaFunction
{
    // error of this method is less than 2*10^(-10)
    private static function GammaLog($x)
    {
         $coefs = array(
            2.5066282746310005,
            1.0000000000190015,
            76.18009172947146,
            -86.50532032941677,
            24.01409824083091,
            -1.231739572450155,
            0.1208650973866179e-2,
            -0.5395239384953e-5,);

        $y = 0;
        $series = 0;
        $j = 0;

        /*calculate the series */
        $series=$coefs[1];
        $y=$x;

        for ($j = 2, $coefCount = count($coefs); $j < $coefCount; ++$j)
        {
            $y+=1.0;
            $series += $coefs[$j] / $y;
        }

        /* and the other parts of the function */
        $y = $x + 5.5;
        $y -= ($x + 0.5) * log($y);

        return (-$y + log($coefs[0] * $series / $x));
     }

     public static function Calculate($x)
     {
         return exp(GammaFunction::GammaLog($x));
     }
}

?>
