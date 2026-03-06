import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatDividerModule } from '@angular/material/divider';
import { MatRippleModule } from '@angular/material/core';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatSlideToggleModule, MatDividerModule, MatRippleModule],
  template: `
    <div class="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
      <!-- Header -->
      <div class="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 class="text-3xl font-bold tracking-tight text-slate-900 dark:text-white">System Settings</h1>
          <p class="text-slate-500 dark:text-slate-400 mt-1">Manage global preferences and account security configurations.</p>
        </div>
        <div class="flex items-center space-x-3">
          <button mat-flat-button color="primary" class="!rounded-xl !px-6 !py-2.5 !bg-indigo-600 !text-white shadow-lg shadow-indigo-600/20 font-bold">
            Save All Changes
          </button>
        </div>
      </div>

      <div class="grid grid-cols-1 xl:grid-cols-3 gap-8">
        <!-- Main Settings Column -->
        <div class="xl:col-span-2 space-y-6">
          
          <!-- General Preferences -->
          <div class="bg-white dark:bg-slate-900/50 backdrop-blur-md rounded-[32px] border border-slate-200 dark:border-slate-800 shadow-sm overflow-hidden">
            <div class="p-6 border-b border-slate-100 dark:border-slate-800 flex items-center space-x-3">
              <div class="w-10 h-10 rounded-xl bg-indigo-50 dark:bg-indigo-500/10 flex items-center justify-center text-indigo-600">
                <mat-icon>settings</mat-icon>
              </div>
              <h3 class="text-lg font-bold text-slate-900 dark:text-white">General Preferences</h3>
            </div>
            
            <div class="p-8 space-y-8">
              <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
                <div class="space-y-2">
                  <label class="text-sm font-bold text-slate-500 uppercase tracking-wider ml-1">Default Language</label>
                  <select class="w-full px-4 py-3.5 bg-slate-50 dark:bg-slate-950 border border-slate-200 dark:border-slate-800 rounded-2xl text-slate-900 dark:text-white outline-none focus:ring-4 focus:ring-indigo-500/10 focus:border-indigo-500 transition-all font-medium appearance-none">
                    <option>English (United States)</option>
                    <option>Arabic (Egypt)</option>
                    <option>French (France)</option>
                  </select>
                </div>
                <div class="space-y-2">
                  <label class="text-sm font-bold text-slate-500 uppercase tracking-wider ml-1">Currency Format</label>
                  <select class="w-full px-4 py-3.5 bg-slate-50 dark:bg-slate-950 border border-slate-200 dark:border-slate-800 rounded-2xl text-slate-900 dark:text-white outline-none focus:ring-4 focus:ring-indigo-500/10 focus:border-indigo-500 transition-all font-medium appearance-none">
                    <option>USD ($) - Dollar</option>
                    <option>EGP (E£) - Pound</option>
                    <option>EUR (€) - Euro</option>
                  </select>
                </div>
              </div>

              <div class="space-y-4 pt-4">
                 <div class="flex items-center justify-between py-2">
                    <div class="space-y-0.5">
                      <p class="font-bold text-slate-900 dark:text-white">Compact Sidebar</p>
                      <p class="text-sm text-slate-500">Reduce sidebar width for more workspace.</p>
                    </div>
                    <mat-slide-toggle color="primary"></mat-slide-toggle>
                 </div>
                 <mat-divider class="dark:!border-slate-800"></mat-divider>
                 <div class="flex items-center justify-between py-2">
                    <div class="space-y-0.5">
                      <p class="font-bold text-slate-900 dark:text-white">Email Daily Digest</p>
                      <p class="text-sm text-slate-500">Receive a summary of all transactions every morning.</p>
                    </div>
                    <mat-slide-toggle [checked]="true" color="primary"></mat-slide-toggle>
                 </div>
              </div>
            </div>
          </div>

          <!-- Security Section -->
          <div class="bg-white dark:bg-slate-900/50 backdrop-blur-md rounded-[32px] border border-slate-200 dark:border-slate-800 shadow-sm overflow-hidden">
            <div class="p-6 border-b border-slate-100 dark:border-slate-800 flex items-center space-x-3">
              <div class="w-10 h-10 rounded-xl bg-rose-50 dark:bg-rose-500/10 flex items-center justify-center text-rose-600">
                <mat-icon>security</mat-icon>
              </div>
              <h3 class="text-lg font-bold text-slate-900 dark:text-white">Account Security</h3>
            </div>
            
            <div class="p-8 space-y-6">
              <div class="flex flex-col md:flex-row md:items-center justify-between gap-4 p-5 bg-slate-50 dark:bg-slate-950 rounded-2xl border border-slate-100 dark:border-slate-800">
                <div class="flex items-start space-x-4">
                  <mat-icon class="text-indigo-500 mt-1">vibration</mat-icon>
                  <div>
                    <p class="font-bold text-slate-900 dark:text-white">Two-Factor Authentication</p>
                    <p class="text-sm text-slate-500">Add an extra layer of security to your account.</p>
                  </div>
                </div>
                <button mat-stroked-button class="!rounded-xl !px-6 !border-indigo-200 dark:!border-indigo-500/20 text-indigo-600 font-bold">
                  Enable
                </button>
              </div>

              <div class="flex flex-col md:flex-row md:items-center justify-between gap-4 p-5 bg-slate-50 dark:bg-slate-950 rounded-2xl border border-slate-100 dark:border-slate-800">
                <div class="flex items-start space-x-4">
                  <mat-icon class="text-slate-400 mt-1">history</mat-icon>
                  <div>
                    <p class="font-bold text-slate-900 dark:text-white">Login History</p>
                    <p class="text-sm text-slate-500">View and manage your active sessions.</p>
                  </div>
                </div>
                <button mat-stroked-button class="!rounded-xl !px-6 !border-slate-200 dark:!border-slate-800 text-slate-600 dark:text-slate-400 font-bold">
                  View Logs
                </button>
              </div>
            </div>
          </div>
        </div>

        <!-- Sidebar / Help Column -->
        <div class="space-y-6">
           <div class="bg-gradient-to-tr from-indigo-600 to-blue-700 rounded-[32px] p-8 text-white shadow-xl relative overflow-hidden group">
             <div class="absolute -right-8 -top-8 w-40 h-40 bg-white/10 rounded-full blur-3xl group-hover:scale-150 transition-transform duration-700"></div>
             <h3 class="text-xl font-bold mb-4 relative z-10">Antigravity Premium</h3>
             <p class="text-indigo-100 mb-6 relative z-10 leading-relaxed font-medium">Your account is currently on the high-fidelity enterprise plan with full API access.</p>
             <button class="w-full py-3.5 bg-white text-indigo-600 font-bold rounded-2xl shadow-xl shadow-indigo-900/20 relative z-10 transform active:scale-95 transition-all">
                Manage Subscription
             </button>
           </div>

           <div class="bg-white dark:bg-slate-900/50 backdrop-blur-md rounded-[32px] border border-slate-200 dark:border-slate-800 p-6 shadow-sm">
             <h4 class="font-bold text-slate-900 dark:text-white mb-4">Quick Help</h4>
             <div class="space-y-3">
               <button matRipple class="w-full text-left px-4 py-3 rounded-2xl hover:bg-slate-50 dark:hover:bg-slate-800 transition-all group">
                 <p class="text-sm font-bold text-slate-700 dark:text-slate-200 group-hover:text-indigo-500 transition-colors">Privacy Policy</p>
                 <p class="text-xs text-slate-400 mt-0.5">How we handle your data</p>
               </button>
               <button matRipple class="w-full text-left px-4 py-3 rounded-2xl hover:bg-slate-50 dark:hover:bg-slate-800 transition-all group">
                 <p class="text-sm font-bold text-slate-700 dark:text-slate-200 group-hover:text-indigo-500 transition-colors">Terms of Service</p>
                 <p class="text-xs text-slate-400 mt-0.5">Our service agreement</p>
               </button>
               <button matRipple class="w-full text-left px-4 py-3 rounded-2xl hover:bg-slate-50 dark:hover:bg-slate-800 transition-all group">
                 <p class="text-sm font-bold text-slate-700 dark:text-slate-200 group-hover:text-indigo-500 transition-colors">Contact Support</p>
                 <p class="text-xs text-slate-400 mt-0.5">24/7 technical assistance</p>
               </button>
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
export class SettingsComponent { }
