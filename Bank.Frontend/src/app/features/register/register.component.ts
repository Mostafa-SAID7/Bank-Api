import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-register',
    standalone: true,
    imports: [
        CommonModule,
        RouterModule,
        MatIconModule,
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule,
        MatCheckboxModule,
        ReactiveFormsModule
    ],
    template: `
    <div class="min-h-screen w-full flex items-center justify-center p-4 bg-[#0F172A] relative overflow-hidden">
      <!-- Animated Background Mesh -->
      <div class="absolute inset-0 z-0">
        <div class="absolute top-[-10%] left-[-10%] w-[40%] h-[40%] bg-indigo-500/10 blur-[120px] rounded-full animate-pulse"></div>
        <div class="absolute bottom-[-10%] right-[-10%] w-[40%] h-[40%] bg-blue-500/10 blur-[120px] rounded-full animate-pulse" style="animation-delay: 2s;"></div>
      </div>

      <!-- Registration Card -->
      <div class="relative z-10 w-full max-w-md">
        <div class="bg-slate-900/50 backdrop-blur-xl border border-white/10 rounded-[32px] p-8 shadow-2xl overflow-hidden relative group">
          <!-- Top Glow -->
          <div class="absolute top-0 left-1/2 -translate-x-1/2 w-3/4 h-[1px] bg-gradient-to-r from-transparent via-indigo-500 to-transparent"></div>
          
          <div class="text-center mb-8">
            <div class="w-16 h-16 bg-gradient-to-tr from-indigo-600 to-blue-500 rounded-2xl flex items-center justify-center mx-auto mb-4 shadow-lg shadow-indigo-500/20 transform group-hover:scale-110 transition-transform duration-500">
              <mat-icon class="!text-3xl text-white">person_add</mat-icon>
            </div>
            <h1 class="text-3xl font-bold text-white tracking-tight">Create Account</h1>
            <p class="text-slate-400 mt-2">Join FinBank and manage your assets with premium tools.</p>
          </div>

          <form [formGroup]="registerForm" (ngSubmit)="onSubmit()" class="space-y-5">
            <!-- Full Name -->
            <div class="space-y-1.5">
              <label class="text-sm font-semibold text-slate-300 ml-1">Full Name</label>
              <div class="relative">
                <mat-icon class="absolute left-4 top-1/2 -translate-y-1/2 text-slate-500 !w-5 !h-5 !text-xl">person</mat-icon>
                <input type="text" formControlName="fullName" placeholder="Ahmed Ali"
                  class="w-full pl-12 pr-4 py-3.5 bg-slate-950/50 border border-slate-800 rounded-2xl text-white placeholder-slate-600 outline-none focus:border-indigo-500 focus:ring-4 focus:ring-indigo-500/10 transition-all">
              </div>
            </div>

            <!-- Email -->
            <div class="space-y-1.5">
              <label class="text-sm font-semibold text-slate-300 ml-1">Email Address</label>
              <div class="relative">
                <mat-icon class="absolute left-4 top-1/2 -translate-y-1/2 text-slate-500 !w-5 !h-5 !text-xl">mail</mat-icon>
                <input type="email" formControlName="email" placeholder="name@company.com"
                  class="w-full pl-12 pr-4 py-3.5 bg-slate-950/50 border border-slate-800 rounded-2xl text-white placeholder-slate-600 outline-none focus:border-indigo-500 focus:ring-4 focus:ring-indigo-500/10 transition-all">
              </div>
            </div>

            <!-- Password -->
            <div class="grid grid-cols-1 gap-5 md:grid-cols-2">
              <div class="space-y-1.5">
                <label class="text-sm font-semibold text-slate-300 ml-1">Password</label>
                <div class="relative">
                  <mat-icon class="absolute left-4 top-1/2 -translate-y-1/2 text-slate-500 !w-5 !h-5 !text-xl">lock</mat-icon>
                  <input [type]="hidePassword ? 'password' : 'text'" formControlName="password" placeholder="••••••••"
                    class="w-full pl-12 pr-12 py-3.5 bg-slate-950/50 border border-slate-800 rounded-2xl text-white placeholder-slate-600 outline-none focus:border-indigo-500 focus:ring-4 focus:ring-indigo-500/10 transition-all">
                  <button type="button" (click)="hidePassword = !hidePassword" class="absolute right-4 top-1/2 -translate-y-1/2 text-slate-500 hover:text-slate-300">
                    <mat-icon class="!w-5 !h-5 !text-xl">{{ hidePassword ? 'visibility_off' : 'visibility' }}</mat-icon>
                  </button>
                </div>
              </div>

              <div class="space-y-1.5">
                <label class="text-sm font-semibold text-slate-300 ml-1">Confirm</label>
                <div class="relative">
                  <mat-icon class="absolute left-4 top-1/2 -translate-y-1/2 text-slate-500 !w-5 !h-5 !text-xl">lock</mat-icon>
                  <input [type]="hideConfirmPassword ? 'password' : 'text'" formControlName="confirmPassword" placeholder="••••••••"
                    class="w-full pl-12 pr-12 py-3.5 bg-slate-950/50 border border-slate-800 rounded-2xl text-white placeholder-slate-600 outline-none focus:border-indigo-500 focus:ring-4 focus:ring-indigo-500/10 transition-all">
                  <button type="button" (click)="hideConfirmPassword = !hideConfirmPassword" class="absolute right-4 top-1/2 -translate-y-1/2 text-slate-500 hover:text-slate-300">
                    <mat-icon class="!w-5 !h-5 !text-xl">{{ hideConfirmPassword ? 'visibility_off' : 'visibility' }}</mat-icon>
                  </button>
                </div>
              </div>
            </div>

            <!-- Terms -->
            <div class="flex items-center space-x-2 py-2">
              <mat-checkbox formControlName="terms" color="primary"></mat-checkbox>
              <span class="text-sm text-slate-400">
                I agree to the <a href="#" class="text-indigo-400 hover:text-indigo-300 font-medium">Terms & Conditions</a>
              </span>
            </div>

            <!-- Submit Button -->
            <button type="submit" [disabled]="registerForm.invalid"
              class="w-full py-4 bg-gradient-to-r from-indigo-600 to-blue-600 hover:from-indigo-500 hover:to-blue-500 disabled:opacity-50 disabled:cursor-not-allowed text-white font-bold rounded-2xl shadow-lg shadow-indigo-500/20 transform active:scale-[0.98] transition-all duration-200">
              Sign Up
            </button>
          </form>

          <!-- Footer -->
          <div class="mt-8 text-center bg-slate-800/10 -mx-8 -mb-8 p-6 border-t border-white/5">
            <p class="text-slate-400 text-sm">
              Already have an account? 
              <a routerLink="/login" class="text-indigo-400 hover:text-indigo-300 font-bold ml-1 transition-colors">Log In</a>
            </p>
          </div>
        </div>

        <!-- System Version Shadow -->
        <p class="text-center mt-6 text-slate-600 text-xs font-mono uppercase tracking-widest">Antigravity Core v1.0.4</p>
      </div>
    </div>
  `,
    styles: `
    :host {
      display: block;
    }
    ::ng-deep .mat-mdc-checkbox .mdc-checkbox__native-control:enabled:checked ~ .mdc-checkbox__background {
      background-color: #6366F1 !important;
      border-color: #6366F1 !important;
    }
    ::ng-deep .mat-mdc-checkbox .mdc-checkbox__native-control:enabled:not(:checked) ~ .mdc-checkbox__background {
      border-color: rgba(255, 255, 255, 0.1) !important;
      background-color: rgba(0, 0, 0, 0.2) !important;
    }
  `
})
export class RegisterComponent {
    registerForm: FormGroup;
    hidePassword = true;
    hideConfirmPassword = true;

    constructor(private fb: FormBuilder) {
        this.registerForm = this.fb.group({
            fullName: ['', [Validators.required]],
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.minLength(6)]],
            confirmPassword: ['', [Validators.required]],
            terms: [false, [Validators.requiredTrue]]
        });
    }

    onSubmit() {
        if (this.registerForm.valid) {
            console.log('Registering...', this.registerForm.value);
        }
    }
}
