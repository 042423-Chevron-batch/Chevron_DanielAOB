


$score = Read-Host -Prompt "enter score here "


if ($score -ge 90){
	$Grade = "A"
}
elseif ($score -ge 80){
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

Write-Output "The grade for the $score is $Grade"




