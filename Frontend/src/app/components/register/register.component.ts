import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  signupForm !: FormGroup;

  constructor(private formBuilder: FormBuilder, private userService: UserService, 
    private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.signupForm = this.formBuilder.group({
      name: ["", [Validators.required]],
      email: ["", [Validators.required, Validators.email]],
      password: ["", [Validators.required, Validators.pattern(/(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*#?&^_-]).{8,}/)]],
      confirmPass: ["", [Validators.required, this.matchValidator]],
    });
  }

  matchValidator(formGroup: FormGroup) {
    const pass = formGroup.get('password');
    const confirmPass = formGroup.get('confirmPass');
    if(pass != confirmPass) {
      return {match: true};
    }
    return null;
  }

  get Name(): FormControl {
    return this.signupForm.get('name') as FormControl;
  }

  get Email(): FormControl {
    return this.signupForm.get('email') as FormControl;
  }

  get Password(): FormControl {
    return this.signupForm.get('password') as FormControl;
  }

  get ConfirmPass(): FormControl {
    return this.signupForm.get('confirmPass') as FormControl;
  }

  register = () => {
    if (this.signupForm.valid) {
      console.log(this.signupForm.value);
      this.userService.register(this.signupForm.value)
        .subscribe({
          next:(response) => {
            this.signupForm.reset();
            if(response.status == "Success") { 
              this.toastr.success("Registered Successfully. Login now to start Shopping" ,"Success");
              this.router.navigate(['/login']);
            } else {
              this.toastr.warning("User with this email already exists." ,"Error");
            }
          },
          error: (err: HttpErrorResponse) => {
            console.log(err);
            this.toastr.error("User registration failed. Please check user details and Try again" ,"Error");
          }
        })
    }
  }
}
