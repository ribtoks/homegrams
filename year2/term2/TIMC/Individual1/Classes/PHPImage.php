<?php
/* 
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of PHPImage
 *
 * @author taras
 */
class PHPImage
{
    //put your code here
    static function image($imageCaption, $phpModuleName)
    {
        print("<strong>$imageCaption</strong><br />");
        print("<img src=\"$phpModuleName\" vspace = \"10\" hspace=\"10\" />");
    }
}
?>
