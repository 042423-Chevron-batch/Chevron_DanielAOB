import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { LogIn } from '../models/LogIn';
import { ApiServices } from '../services/api.service';
import { Register } from '../models/Register';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent {

  constructor(private apiService: ApiServices) { }



  RegisterUser: Register = { FirstName: '', LastName: '', UserName: '', Email: ' ', Password: ' ' };


  onRegSubmit() {



    // Create a new login object with the entered values
    //const RegisterUser: Register = new Register(LastName, FirstName, Username, EmailAdd, Password);

    // Call the login method of the API service and pass the login object
    this.apiService.registration(this.RegisterUser).subscribe(
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
