<?php
/* 
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

require_once 'HTMLTable.php';

/**
 * Description of WarningProvider
 *
 * @author taras
 */
class WarningProvider
{
    //put your code here
    static function Warn($text)
    {
        echo '<table cellspacing="5" cellpadding="7" style="border-width: 1px;
	border-style: solid;
	border-color: black;
	border-collapse: collapse;
	background-color: white;">';

        echo '<tr><td><img src="Images/Process-stop.png" /></td>';

        echo '<td style="background-color:#C01717;">
                <span style="color:white; font-weight:bold; font-size:20px;
             letter-spacing:1.5px;">Warning</span></td>';

        echo "<td><strong>$text</strong></td>";
        echo "</tr>";

        HTMLTable::end();
    }
}
?>
