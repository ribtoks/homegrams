<?php
/*
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title></title>
    </head>
    <body>
  */

        function mysin($x)
        {
            return sin($x);
        }

        function square($x)
        {
            return $x*$x;
        }


        require_once 'DiscretePlotter.php';
        require_once 'ContinuousPlotter.php';
        // put your code here
        $k = 12;

        $data = array(1987, 5230, 2764, 2993, 3892,
                      2890 + $k, 2984 - $k, 4290 - $k, 3418 + $k, 5000 + $k,
                      3860, 4098, 3654, 3218, 3140,
                      4216, 5633, 5139, 4076, 5880,
                      4987, 2691, 4515, 1374, 3911,
                      4421, 1673, 2116, 2809, 4367);


        $otherData = array(0, 0, 1, 0, 0,
                           0, 1, 1, 0, 0,
                           0, 1, 0, 0, 0,
                           1, 0, 0, 1, 0,
                           1, 0, 1, 0, 0,
                           0, 0, 1, 0, 0);

        $otherData2 = array(-14, 13, 1, -8, 0,
                           24, 1, 23, 7, 2,
                           21, 42, 0, 0, 3,
                           1, -12, 0, 1, 48,
                           12, 45, 1, 28, 9,
                           23, 14, 1, -5, -3);

        $otherData3 = array(-14, -13, -1, -8, 0,
                           -24, -1, -23, -7, -2,
                           -21, -42, 0, 0, -3,
                           -1, -12, 0, -1, -48,
                           -12, -45, -1, -28, -9,
                           -23, -14, -1, -5, -3);



        global $AvailableColors;

        //$plotter = new DiscretePlotter($data, array($otherData, $otherData2, $otherData3),
        //    'Global title', 'white', $AvailableColors, 800, 600, 3);

        $plotter = new ContinuousPlotter(-2, 2, array('square', 'mysin'),
            'Global title', 'white', $AvailableColors, 800, 600, 3);

        $plotter->ProcessGraphics();

        $plotter->OutGraphics(0);
        //$plotter->OutGraphics(1);
    /*
    </body>
</html>
     * 
     */
?>