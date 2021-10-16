<?php
$con = mysqli_connect ('localhost', 'root', '', 'unityaccess');

if (mysqli_connect_errno())
{
    echo "1: Connection failed"; //error code #1
    exit();
}

$username = $_POST["name"];
$newscore = $_POST["score"];

//check if name exists
$namecheckquery = "SELECT username FROM players WHERE username='" . $username . "';";

$namecheck = mysqli_query($con, $namecheckquery) or die("2: Name check query failed"); //error code #2

if (mysqli_num_rows($namecheck) != 1)
{
    echo "5: Either no user with name or more than one"; //error code #3
    exit();
}

$updatequery = "UPDATE players SET score = " . $newscore . " WHERE username = '" . $username . "';";
mysqli_query($con, $updatequery) or die ("er 7: coulndt update score");
echo "0";
?>