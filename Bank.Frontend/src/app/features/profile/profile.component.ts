import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatRippleModule } from '@angular/material/core';
import { MatTabsModule } from '@angular/material/tabs';

@Component({
    selector: 'app-profile',
    standalone: true,
    imports: [
        CommonModule,
        MatIconModule,
        MatButtonModule,
        MatRippleModule,
        MatTabsModule
    ],
    template: `
    <div class="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
      <!-- Header Section -->
      <div class="flex flex-col md:flex-row md:items-end justify-between gap-6 pb-2 border-b border-slate-200 dark:border-slate-800">
        <div class="flex items-center space-x-5">
          <div class="relative group">
            <div class="w-24 h-24 rounded-[2rem] overflow-hidden border-4 border-white dark:border-slate-900 shadow-xl relative z-10 transition-transform group-hover:scale-105 duration-500">
              <img src="https://ui-avatars.com/api/?name=Ahmed+Ali&background=6366f1&color=fff&size=200" alt="Profile" class="w-full h-full object-cover">
            </div>
            <button class="absolute bottom-0 right-0 z-20 w-8 h-8 bg-indigo-600 text-white rounded-xl shadow-lg border-2 border-white dark:border-slate-900 flex items-center justify-center hover:bg-indigo-500 transition-colors">
              <mat-icon class="!text-lg">photo_camera</mat-icon>
            </button>
            <div class="absolute inset-0 bg-indigo-500/20 blur-2xl rounded-full opacity-0 group-hover:opacity-100 transition-opacity duration-500"></div>
          </div>
          <div>
            <h1 class="text-3xl font-bold text-slate-900 dark:text-white leading-tight">Ahmed Ali</h1>
            <p class="text-slate-500 dark:text-slate-400 font-medium flex items-center mt-1">
              <mat-icon class="!w-4 !h-4 !text-base mr-1 text-slate-400">verified</mat-icon>
              Verified Premium Account
            </p>
          </div>
        </div>
        <div class="flex items-center space-x-3">
          <button mat-stroked-button color="primary" class="!rounded-xl !px-6 !py-2.5 !border-slate-200 dark:!border-slate-800 hover:!bg-slate-50 dark:hover:!bg-slate-800 transition-all font-semibold">
            Cancel
          </button>
          <button mat-flat-button color="primary" class="!rounded-xl !px-8 !py-2.5 !bg-indigo-600 !text-white hover:!bg-indigo-500 transition-all shadow-lg shadow-indigo-600/20 font-bold tracking-wide">
            Save Changes
          </button>
        </div>
      </div>

      <!-- Main Content Grid -->
      <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        
        <!-- Left Column: Navigation/Summary -->
        <div class="lg:col-span-1 space-y-6">
          <div class="bg-white dark:bg-slate-900/50 backdrop-blur-md rounded-3xl border border-slate-200 dark:border-slate-800 p-6 shadow-sm overflow-hidden relative group">
            <div class="absolute top-0 right-0 p-4 opacity-5 group-hover:scale-110 transition-transform duration-700">
              <mat-icon class="!text-8xl">account_circle</mat-icon>
            </div>

            <nav class="space-y-1 relative z-10">
              <button matRipple class="w-full flex items-center space-x-3 px-4 py-3 bg-indigo-50 dark:bg-indigo-500/10 text-indigo-600 dark:text-indigo-400 rounded-2xl font-bold transition-all">
                <mat-icon>person</mat-icon>
                <span>General Profile</span>
              </button>
              <button matRipple class="w-full flex items-center space-x-3 px-4 py-3 text-slate-600 dark:text-slate-400 hover:bg-slate-50 dark:hover:bg-slate-800 rounded-2xl font-medium transition-all group/btn">
                <mat-icon class="group-hover/btn:text-indigo-500">shield</mat-icon>
                <span>Security & Password</span>
              </button>
              <button matRipple class="w-full flex items-center space-x-3 px-4 py-3 text-slate-600 dark:text-slate-400 hover:bg-slate-50 dark:hover:bg-slate-800 rounded-2xl font-medium transition-all group/btn">
                <mat-icon class="group-hover/btn:text-indigo-500">notifications</mat-icon>
                <span>Notification Settings</span>
              </button>
              <button matRipple class="w-full flex items-center space-x-3 px-4 py-3 text-slate-600 dark:text-slate-400 hover:bg-slate-50 dark:hover:bg-slate-800 rounded-2xl font-medium transition-all group/btn">
                <mat-icon class="group-hover/btn:text-indigo-500">history</mat-icon>
                <span>Session Log</span>
              </button>
            </nav>
          </div>

          <!-- Account Health Card -->
          <div class="bg-gradient-to-br from-slate-900 to-indigo-900 rounded-3xl p-6 text-white shadow-xl relative overflow-hidden group">
            <div class="absolute top-0 right-0 w-32 h-32 bg-indigo-400/10 rounded-full blur-3xl -mr-16 -mt-16 group-hover:scale-150 transition-transform duration-700"></div>
            <h3 class="text-lg font-bold mb-4 flex items-center">
              <mat-icon class="mr-2 text-indigo-400">bolt</mat-icon>
              Account Quality
            </h3>
            <div class="space-y-4">
              <div class="flex justify-between text-sm mb-1.5 font-medium">
                <span class="text-slate-300">Profile Completion</span>
                <span class="text-white">85%</span>
              </div>
              <div class="h-2 w-full bg-slate-800/50 rounded-full overflow-hidden">
                <div class="h-full bg-indigo-500 rounded-full shadow-[0_0_10px_rgba(99,102,241,0.5)]" style="width: 85%"></div>
              </div>
              <p class="text-xs text-slate-400 leading-relaxed italic">
                Tips: Add your phone number and verify your identity to reach 100%.
              </p>
            </div>
          </div>
        </div>

        <!-- Right Column: Forms -->
        <div class="lg:col-span-2 space-y-6">
          <div class="bg-white dark:bg-slate-900/50 backdrop-blur-md rounded-3xl border border-slate-200 dark:border-slate-800 p-8 shadow-sm">
            <div class="flex items-center justify-between mb-8">
              <div>
                <h3 class="text-xl font-bold text-slate-900 dark:text-white">Personal Information</h3>
                <p class="text-slate-500 dark:text-slate-400 text-sm mt-1">Manage your identity and public details.</p>
              </div>
              <div class="flex items-center py-1.5 px-3 bg-emerald-50 dark:bg-emerald-500/10 text-emerald-600 dark:text-emerald-400 rounded-xl text-xs font-bold uppercase tracking-wider">
                <div class="w-1.5 h-1.5 bg-emerald-500 rounded-full mr-2 animate-pulse"></div>
                Auto-Saved
              </div>
            </div>

            <div class="grid grid-cols-1 md:grid-cols-2 gap-x-8 gap-y-6 text-left">
              <div class="space-y-1.5">
                <label class="text-sm font-semibold text-slate-600 dark:text-slate-400 ml-1">First Name</label>
                <input type="text" value="Ahmed"
                  class="w-full px-5 py-3.5 bg-slate-50/50 dark:bg-slate-950/50 border border-slate-200 dark:border-slate-800 rounded-2xl text-slate-900 dark:text-white placeholder-slate-400 outline-none focus:border-indigo-500 focus:ring-4 focus:ring-indigo-500/10 transition-all font-medium">
              </div>
              <div class="space-y-1.5">
                <label class="text-sm font-semibold text-slate-600 dark:text-slate-400 ml-1">Last Name</label>
                <input type="text" value="Ali"
                  class="w-full px-5 py-3.5 bg-slate-50/50 dark:bg-slate-950/50 border border-slate-200 dark:border-slate-800 rounded-2xl text-slate-900 dark:text-white placeholder-slate-400 outline-none focus:border-indigo-500 focus:ring-4 focus:ring-indigo-500/10 transition-all font-medium">
              </div>
              <div class="space-y-1.5">
                <label class="text-sm font-semibold text-slate-600 dark:text-slate-400 ml-1">Email Address</label>
                <div class="relative">
                  <input type="email" value="ahmed.ali@finbank.com"
                    class="w-full px-5 py-3.5 bg-slate-50/50 dark:bg-slate-950/50 border border-slate-200 dark:border-slate-800 rounded-2xl text-slate-900 dark:text-white placeholder-slate-400 outline-none focus:border-indigo-500 focus:ring-4 focus:ring-indigo-500/10 transition-all font-medium opacity-80 cursor-not-allowed" readonly>
                  <mat-icon class="absolute right-4 top-1/2 -translate-y-1/2 text-emerald-500 !w-5 !h-5 !text-xl">verified</mat-icon>
                </div>
              </div>
              <div class="space-y-1.5">
                <label class="text-sm font-semibold text-slate-600 dark:text-slate-400 ml-1">Phone Number</label>
                <input type="text" value="+20 123 456 7890"
                  class="w-full px-5 py-3.5 bg-slate-50/50 dark:bg-slate-950/50 border border-slate-200 dark:border-slate-800 rounded-2xl text-slate-900 dark:text-white placeholder-slate-400 outline-none focus:border-indigo-500 focus:ring-4 focus:ring-indigo-500/10 transition-all font-medium">
              </div>
              <div class="md:col-span-2 space-y-1.5">
                <label class="text-sm font-semibold text-slate-600 dark:text-slate-400 ml-1">Location / Address</label>
                <input type="text" value="Maadi, Cairo, Egypt"
                  class="w-full px-5 py-3.5 bg-slate-50/50 dark:bg-slate-950/50 border border-slate-200 dark:border-slate-800 rounded-2xl text-slate-900 dark:text-white placeholder-slate-400 outline-none focus:border-indigo-500 focus:ring-4 focus:ring-indigo-500/10 transition-all font-medium">
              </div>
              <div class="md:col-span-2 space-y-1.5 pt-4">
                 <button mat-stroked-button color="warn" class="!rounded-2xl !px-6 !py-2.5 !border-rose-100 dark:!border-rose-500/10 !bg-rose-50 dark:!bg-rose-500/5 !text-rose-600 dark:!text-rose-400 hover:!bg-rose-100 dark:hover:!bg-rose-500/10 transition-all font-bold tracking-wide">
                    Deactivate My Account
                 </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
    styles: `
    :host {
      display: block;
    }
  `
})
export class ProfileComponent { }
