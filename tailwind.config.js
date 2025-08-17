/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Pages/**/*.{cshtml,razor}",   
    "./Views/**/*.{cshtml,razor}",   
    "./wwwroot/js/**/*.js"           
  ],
  theme: {
    extend: {
      fontFamily: {
        display: ['ui-sans-serif', 'system-ui', 'Segoe UI', 'Roboto', 'Inter', 'sans-serif'],
      },
      colors: {
        brand: {
          DEFAULT: '#4f46e5',  
          light: '#6366f1',
          dark: '#3730a3'
        }
      }
    },
  },
  plugins: [],
}
