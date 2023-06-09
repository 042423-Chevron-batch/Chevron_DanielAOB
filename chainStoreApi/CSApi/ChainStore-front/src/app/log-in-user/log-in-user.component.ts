import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { LogIn } from '../models/LogIn';
import { ApiServices } from '../api.service';

@Component({
  selector: 'app-log-in-user',
  templateUrl: './log-in-user.component.html',
  styleUrls: ['./log-in-user.component.css']
})
export class LogInUserComponent {


  constructor(private apiService: ApiServices) { }

  login: LogIn = { UserName: '', Password: '' };

  onLogInSubmit() {
    // // Get the entered values from the form controls
    // const usernameValue = this.userName.value;
    // const passwordValue = this.Userpassword.value;


    // Call the login method of the API service and pass the login object
    this.apiService.login(this.login).subscribe(
      (response: any) => {
        // Handle the response from the API
        console.log(response);
      },
      (error: any) => {
        // Handle any errors that occurred during the API call
        console.error(error);
      }
    );
  }
}
