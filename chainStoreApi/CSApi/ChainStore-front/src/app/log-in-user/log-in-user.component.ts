import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { LogIn } from '../models/LogIn';
import { ApiServices } from '../services/api.service';
import { Person } from '../models/Person';
import { AuthService } from '../services/auth.service';


@Component({
  selector: 'app-log-in-user',
  templateUrl: './log-in-user.component.html',
  styleUrls: ['./log-in-user.component.css']
})
export class LogInUserComponent {


  constructor(private apiService: ApiServices, private auth: AuthService) { }

  login: LogIn = { UserName: '', Password: '' };


  personDetails: Person = {
    CustomerId: "",
    fname: "",
    lname: "",
    userName: "",
    email: "",
    password: ""

  }

  onLogInSubmit() {
    // // Get the entered values from the form controls
    // const usernameValue = this.userName.value;
    // const passwordValue = this.Userpassword.value;


    // Call the login method of the API service and pass the login object
    this.apiService.login(this.login).subscribe(
      (response: Person) => {
        // Handle the response from the API
        this.personDetails = JSON.parse(JSON.stringify(response));

        this.auth.setCurrentUser(this.personDetails)

        //this.personDetails = response;
        console.log("You are welcome ", + this.personDetails.fname);

      },
      (error: any) => {
        // Handle any errors that occurred during the API call
        console.error(error);
      }
    );
  }
}
