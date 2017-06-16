<?php
/* 
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

function Power($a, $n)
{
    $x = $a;
    $r = 1;
    while ($n)
    {
        if ($n & 1)
            $r *= $x;

        $n >>= 1;
        $x *= $x;
    }
    return $r;
}

function Factorial($n)
{
    $product = 1;
    for ($i = 2; $i <= $n; ++$i)
        $product *= $i;

    return $product;
}

?>
