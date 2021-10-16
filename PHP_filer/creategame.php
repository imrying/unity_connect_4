<?php
$con = mysqli_connect ('localhost', 'root', '', 'unityaccess');

if (mysqli_connect_errno())
{
    echo "1: Connection failed"; //error code #1
    exit();
}


$p1 = $_POST["p1"];
$p2 = $_POST["p2"];

$namecheckquery = "SELECT username FROM players WHERE username='" . $p2 . "';";

$namecheck = mysqli_query($con, $namecheckquery) or die("2: Name check query failed"); //error code #2

if (mysqli_num_rows($namecheck) < 1)
{
    echo "3: p2 name doesent exist"; //error code #3
    exit();
}


//add game to the table

$insertgamequery = "INSERT INTO games (p1, p2) VALUES ('" . $p1 . "', '" . $p2 . "');";
mysqli_query($con, $insertgamequery) or die("4: Insert game query failed"); //error code #4 insert query failed
echo("0");
?>