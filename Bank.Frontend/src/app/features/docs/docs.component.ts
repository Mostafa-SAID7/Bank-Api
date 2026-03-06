import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatRippleModule } from '@angular/material/core';

@Component({
    selector: 'app-docs',
    standalone: true,
    imports: [CommonModule, MatIconModule, MatButtonModule, MatRippleModule],
    template: `
    <div class="space-y-12 pb-20 animate-in fade-in slide-in-from-bottom-4 duration-1000">
      <!-- Intro Section -->
      <div class="max-w-3xl">
        <h1 class="text-5xl font-extrabold tracking-tight text-slate-900 dark:text-white leading-tight">
          Antigravity <span class="text-indigo-600 dark:text-indigo-400">Design System</span>
        </h1>
        <p class="text-xl text-slate-500 dark:text-slate-400 mt-4 leading-relaxed">
          A premium, high-performance UI framework for the next generation of fintech applications. 
          Built on top of Tailwind CSS and Angular Material.
        </p>
      </div>

      <!-- Colors Section -->
      <section class="space-y-6">
        <h2 class="text-2xl font-bold text-slate-900 dark:text-white flex items-center">
          <mat-icon class="mr-3 text-indigo-500">palette</mat-icon>
          Color Palette
        </h2>
        <div class="grid grid-cols-2 md:grid-cols-5 gap-6">
          <div *ngFor="let color of colors" class="space-y-3 group">
            <div [class]="'h-24 rounded-3xl shadow-lg transform group-hover:scale-105 transition-transform duration-500 ' + color.class"></div>
            <div class="px-1">
              <p class="text-sm font-bold text-slate-900 dark:text-white uppercase tracking-wider">{{color.name}}</p>
              <p class="text-xs text-slate-400 font-mono mt-0.5">{{color.hex}}</p>
            </div>
          </div>
        </div>
      </section>

      <!-- Typography -->
      <section class="space-y-6">
        <h2 class="text-2xl font-bold text-slate-900 dark:text-white flex items-center">
          <mat-icon class="mr-3 text-indigo-500">text_fields</mat-icon>
          Typography
        </h2>
        <div class="bg-white dark:bg-slate-900/50 p-8 rounded-[40px] border border-slate-200 dark:border-slate-800 space-y-8 divide-y dark:divide-slate-800 shadow-sm">
          <div class="pb-8">
            <h1 class="text-4xl font-extrabold text-slate-900 dark:text-white">Display Heading 1</h1>
            <p class="text-slate-400 mt-2 font-mono text-xs">text-4xl font-extrabold</p>
          </div>
          <div class="py-8">
            <h2 class="text-2xl font-bold text-slate-900 dark:text-white tracking-tight">System Section Title</h2>
            <p class="text-slate-400 mt-2 font-mono text-xs">text-2xl font-bold tracking-tight</p>
          </div>
          <div class="py-8">
            <p class="text-lg text-slate-600 dark:text-slate-300 leading-relaxed font-medium"> 
              The quick brown fox jumps over the lazy dog. Standard body text for high readability.
            </p>
            <p class="text-slate-400 mt-2 font-mono text-xs">text-lg font-medium leading-relaxed</p>
          </div>
          <div class="pt-8">
            <p class="text-xs font-bold text-slate-500 uppercase tracking-[0.2em]">Uppercase Tracking Label</p>
            <p class="text-slate-400 mt-2 font-mono text-xs">text-xs font-bold uppercase tracking-[0.2em]</p>
          </div>
        </div>
      </section>

      <!-- Components Preview -->
      <section class="space-y-6">
        <h2 class="text-2xl font-bold text-slate-900 dark:text-white flex items-center">
          <mat-icon class="mr-3 text-indigo-500">widgets</mat-icon>
          Core Components
        </h2>
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          <!-- Glassmorphic Card -->
          <div class="bg-slate-900/50 backdrop-blur-xl border border-white/10 rounded-3xl p-6 text-white overflow-hidden relative group h-[200px] flex flex-col justify-end">
            <div class="absolute top-0 right-0 p-8 opacity-10 group-hover:scale-125 transition-transform duration-700">
              <mat-icon class="!text-9xl">auto_awesome</mat-icon>
            </div>
            <h4 class="text-xl font-bold">Glassmorphism</h4>
            <p class="text-slate-400 text-sm mt-1 leading-snug">Blured background with subtle white borders.</p>
          </div>

          <!-- Action Buttons -->
          <div class="bg-white dark:bg-slate-900/50 p-6 rounded-3xl border border-slate-200 dark:border-slate-800 flex flex-col items-center justify-center space-y-4 h-[200px]">
            <button mat-flat-button color="primary" class="!rounded-2xl !px-10 !py-6 !bg-indigo-600 !text-white shadow-xl shadow-indigo-600/20 font-bold transition-all hover:translate-y-[-2px]">
              Primary Action
            </button>
            <button mat-stroked-button class="!rounded-2xl !px-10 !py-6 !border-slate-200 dark:!border-slate-800 dark:!text-slate-300 font-bold transition-all hover:bg-slate-50 dark:hover:bg-slate-800">
              Secondary Ghost
            </button>
          </div>

          <!-- Status Chips -->
          <div class="bg-white dark:bg-slate-900/50 p-6 rounded-3xl border border-slate-200 dark:border-slate-800 flex flex-wrap gap-3 items-center justify-center h-[200px]">
            <span class="px-4 py-1.5 bg-emerald-50 dark:bg-emerald-500/10 text-emerald-600 dark:text-emerald-400 text-xs font-extrabold rounded-full uppercase tracking-wider border border-emerald-100 dark:border-emerald-500/20">Success</span>
            <span class="px-4 py-1.5 bg-rose-50 dark:bg-rose-500/10 text-rose-600 dark:text-rose-400 text-xs font-extrabold rounded-full uppercase tracking-wider border border-rose-100 dark:border-rose-500/20">Critical</span>
            <span class="px-4 py-1.5 bg-amber-50 dark:bg-amber-500/10 text-amber-600 dark:text-amber-400 text-xs font-extrabold rounded-full uppercase tracking-wider border border-amber-100 dark:border-amber-500/20">Pending</span>
            <span class="px-4 py-1.5 bg-indigo-50 dark:bg-indigo-500/10 text-indigo-600 dark:text-indigo-400 text-xs font-extrabold rounded-full uppercase tracking-wider border border-indigo-100 dark:border-indigo-500/20">Processing</span>
          </div>
        </div>
      </section>

      <!-- Footer Call to Action -->
      <div class="bg-indigo-600 rounded-[48px] p-12 text-center text-white shadow-2xl shadow-indigo-600/30 relative overflow-hidden group">
        <div class="absolute top-0 left-0 w-full h-full bg-[radial-gradient(circle_at_center,_var(--tw-gradient-stops))] from-indigo-400/20 via-transparent to-transparent"></div>
        <div class="relative z-10 max-w-2xl mx-auto">
          <h3 class="text-3xl font-extrabold mb-4">Ready to build the future?</h3>
          <p class="text-indigo-100 mb-8 leading-relaxed">Antigravity is fully optimized for speed, accessibility, and high-fidelity aesthetics.</p>
          <div class="flex flex-wrap justify-center gap-4">
             <button mat-flat-button class="!bg-white !text-indigo-600 !rounded-[20px] !px-10 !py-7 !font-extrabold !text-lg !shadow-xl">
                Explore Components
             </button>
             <button mat-stroked-button class="!border-white/30 !text-white !rounded-[20px] !px-10 !py-7 !font-extrabold !text-lg hover:!bg-white/10">
                Read API Docs
             </button>
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
export class DocsComponent {
    colors = [
        { name: 'Primary Indigo', hex: '#6366F1', class: 'bg-indigo-500 shadow-indigo-500/20' },
        { name: 'Vibrant Blue', hex: '#3B82F6', class: 'bg-blue-500 shadow-blue-500/20' },
        { name: 'Dark Slate', hex: '#0F172A', class: 'bg-slate-900 border border-white/5' },
        { name: 'Emerald Peak', hex: '#10B981', class: 'bg-emerald-500 shadow-emerald-500/20' },
        { name: 'Rose Impact', hex: '#F43F5E', class: 'bg-rose-500 shadow-rose-500/20' }
    ];
}
