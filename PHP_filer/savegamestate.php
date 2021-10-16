<?php
$con = mysqli_connect ('localhost', 'root', '', 'unityaccess');

if (mysqli_connect_errno())
{
    echo "1: Connection failed"; //error code #1
    exit();
}

$id = $_POST["id"];
$history = $_POST["history"];

$gamecheckquery = "SELECT id FROM games WHERE id='" . $id . "';";

$gamecheck = mysqli_query($con, $gamecheckquery) or die("2: Game check query failed"); //error code #2

if (mysqli_num_rows($gamecheck) < 1)
{
    echo "3: game doesent exist"; //error code #3
    exit();
}


$updatequery = "UPDATE games SET history = " . $history . " WHERE id = '" . $id . "';";
mysqli_query($con, $updatequery) or die ("er 7: coulndt save game");
echo "0";
?>

