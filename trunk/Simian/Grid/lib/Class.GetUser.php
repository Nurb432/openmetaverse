<?php
/** Simian grid services
 *
 * PHP version 5
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 *
 * @package    SimianGrid
 * @author     Jim Radford <http://www.jimradford.com/>
 * @copyright  Open Metaverse Foundation
 * @license    http://www.debian.org/misc/bsd.license  BSD License (3 Clause)
 * @link       http://openmetaverse.googlecode.com/
 */
interface_exists('IGridService') || require_once ('Interface.GridService.php');
class_exists('UUID') || require_once ('Class.UUID.php');

class GetUser implements IGridService
{
    private $UserID;

    public function Execute($db, $params, $logger)
    {
        // This query pre-assembles some of the data into JSON form for speed.
        // If we wanted to return data in a different transport format, a
        // different query would be needed
        $sql = "SELECT Users.ID AS ID, Users.Name, Users.Email,
        		GROUP_CONCAT(CONCAT('\"', UserData.`Key`, '\":'), CONCAT('\"', UserData.`Value`, '\"'))
        		AS ExtraData FROM Users LEFT OUTER JOIN UserData ON Users.ID = UserData.ID";
        $values = array();
        
        if (isset($params["UserID"]) && UUID::TryParse($params["UserID"], $this->UserID))
        {
            $sql .= " WHERE Users.ID=:UserID GROUP BY ID";
            $values["UserID"] = $this->UserID;
        }
        else if (isset($params["Name"]))
        {
            $sql .= " WHERE Name=:Name GROUP BY ID";
            $values["Name"] = $params["Name"];
        }
        else if (isset($params["Email"]))
        {
            $sql .= " WHERE Email=:Email GROUP BY ID";
            $values["Email"] = $params["Email"];
        }
        else
        {
            header("Content-Type: application/json", true);
            echo '{ "Message": "Missing or invalid parameters" }';
            exit();
        }
        
        $sth = $db->prepare($sql);
        
        if ($sth->execute($values))
        {
            if ($sth->rowCount() > 0)
            {
                $obj = $sth->fetchObject();
                
                $output = sprintf('{ "Success": true, "User": { "UserID":"%s","Name":"%s","Email":"%s"',
                    $obj->ID, $obj->Name, $obj->Email);
                if (strlen($obj->ExtraData) > 0)
                    $output .= ',' . $obj->ExtraData;
                $output .= '} }';
                
                header("Content-Type: application/json", true);
                echo $output;
                exit();
            }
            else
            {
                header("Content-Type: application/json", true);
                echo '{ "Message": "No matching user found" }';
                exit();
            }
        }
        else
        {
            $logger->err(sprintf("Error occurred during query: %d %s", $sth->errorCode(), print_r($sth->errorInfo(), true)));
            header("Content-Type: application/json", true);
            echo '{ "Message": "Database query error" }';
            exit();
        }
    }
}
