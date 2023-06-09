import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';



export interface Register {

    FirstName: string;
    LastName: string;
    UserName: string;
    Email: string;
    Password: string;
}




