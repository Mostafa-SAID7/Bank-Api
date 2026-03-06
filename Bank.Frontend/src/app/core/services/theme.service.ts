import { Injectable, signal, effect } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class ThemeService {
    private readonly THEME_KEY = 'bank-simulator-theme';
    private isDarkModeSignal = signal<boolean>(this.getInitialTheme());

    public isDarkMode = this.isDarkModeSignal.asReadonly();

    constructor() {
        // Synchronize to DOM and localStorage
        effect(() => {
            const dark = this.isDarkModeSignal();
            if (dark) {
                document.documentElement.classList.add('dark');
                localStorage.setItem(this.THEME_KEY, 'dark');
            } else {
                document.documentElement.classList.remove('dark');
                localStorage.setItem(this.THEME_KEY, 'light');
            }
        });
    }

    toggleTheme() {
        this.isDarkModeSignal.update(dark => !dark);
    }

    private getInitialTheme(): boolean {
        const saved = localStorage.getItem(this.THEME_KEY);
        if (saved) {
            return saved === 'dark';
        }
        // Check system preference
        return window.matchMedia('(prefers-color-scheme: dark)').matches;
    }
}
