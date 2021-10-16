<?php
$con = mysqli_connect ('localhost', 'root', '', 'unityaccess');

if (mysqli_connect_errno())
{
    echo "1: Connection failed"; //error code #1
    exit();
}

$p1 = $_POST["p1"];

//check if name exists
$query = "SELECT p1, p2, id, history FROM games WHERE p1='" . $p1 . "' OR p2='" . $p1 . "';";
$result = mysqli_query($con, $query);

$json = mysqli_fetch_all($result, MYSQLI_ASSOC);
echo json_encode($json);
//$gamequery = "SELECT p1, p2, id, history FROM games WHERE p1='" . $p1 . "';";


//$gamecheck = mysqli_query($con, $gamequery) or die("2: game check query failed"); //error code #2



?>