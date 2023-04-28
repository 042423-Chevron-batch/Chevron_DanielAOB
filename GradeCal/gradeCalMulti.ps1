
foreach ($score in $args){

if ($score -ge 90) {

        $Grade="A"}
elseif($score -ge 80){
        $Grade = "B"
}
elseif ($score -ge 70){
        $Grade = "C"
}
elseif ($score -ge 60){
        $Grade = "D"
}
else {
        $Grade = "F"
        }
write-output "$score :  $Grade " >> "Grade_score.txt"

}

