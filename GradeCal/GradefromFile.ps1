


Get-Content input.txt | ForEach-Object {

$line = $_

foreach ($score in $line.Split(" ")){

	if ($score -ge 90) {

        $Grade="A"
	}

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
	write-output "$score :  $Grade " >> "Grade_Fromfile_score.txt"
	

}
}








