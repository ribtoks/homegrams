<?php
/* 
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

        require_once 'Classes/IntegralSolver.php';
        
        $r = $_REQUEST['r'];
        $alpha = $_REQUEST['alpha'];
        $epsilon = $_REQUEST['epsilon'];

        echo "<br /><br />";

        $start = microtime(true);

        $integralSolver = new IntegralSolver($epsilon);
        
        echo "Chi Square Critical Value (r=$r, alpha=$alpha) is ";
        
        echo $integralSolver->SolveChiSquare($r, $alpha);

        $end = microtime(true);

        echo "<br /><br />";
        echo "Time elapsed: ";
        echo ($end - $start);
        echo " second(s)";

        include 'ChiCriticalCalc.html';
?>
