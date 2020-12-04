<?php
require_once("config.php");
if(isset($_POST) && !empty($_POST)) {
    
    $DB = mysqli_connect(DBHOST, DBUSERNAME, DBPASSWORD, DBNAME);

    switch($_GET["id"]) {

        case "players":
            $query = "SELECT * FROM `players`";
            $result = mysqli_query($DB, $query);
            if(mysqli_num_rows($result) > 0) {
                while($player = mysqli_fetch_assoc($result)) {
                    print_r($player);
                    echo"<br>";
                }
            }
        break;
        case "matches":
            $query = "SELECT * FROM `matches`";
            $result = mysqli_query($DB, $query);
            if(mysqli_num_rows($result) > 0) {
                while($match = mysqli_fetch_assoc($result)) {
                    print_r($match);
                    echo"<br>";
                }
            }
        break;
        case "playersave":
            $player1 = $_POST["player1"];
            $player2 = isset($_POST["player2"]) ? $_POST["player2"] : null;
            $query = "SELECT * FROM `players` WHERE name='$player1'";
            $result = mysqli_query($DB, $query);
            if(mysqli_num_rows($result) == 0) {
                $query = "INSERT INTO players(id,name) VALUES (null, '$player1')";
                $result = mysqli_query($DB, $query) or die(mysqli_error($DB));
            }
            if ($player2) {
                $query = "SELECT * FROM `players` WHERE name='$player2'";
                $result = mysqli_query($DB, $query);
                if(mysqli_num_rows($result) == 0) {
                    $query = "INSERT INTO players(id,name) VALUES (null, '$player2')";
                    $result = mysqli_query($DB, $query) or die(mysqli_error($DB));
                }
            }
        break;
        case "matchsave":
        
        $player1 = $_POST["player1"];
        $query = "SELECT * FROM `players` WHERE name='$player1'";
        $result = mysqli_query($DB, $query);
        $player1 = mysqli_fetch_array($result);
        $player1 = $player1['id'];

        $player2 = $_POST["player2"];

        if ($player2 == "Computer") {
            $player2 = 0;
        } else {
            $player2 = $_POST["player2"];
            $query = "SELECT * FROM `players` WHERE name='$player2'";
            $result = mysqli_query($DB, $query);
            $player2 = mysqli_fetch_array($result);
            $player2 = $player2['id'];
        }

        $player1Score = $_POST["player1Score"];
        $player2Score = $_POST["player2Score"];
        $mostCommonChoice = $_POST["mostCommonChoice"];
        $query = "INSERT INTO matches(id, player1, player2, player1_round_score, player2_round_score, most_common_choice) VALUES (null, $player1, $player2, '$player1Score', '$player2Score', '$mostCommonChoice')";
        $result = mysqli_query($DB, $query) or die(mysqli_error($DB));
        break;
        default:
        echo "this is default";
        break;
        
    }

} else {

    echo "Access Denied";
}