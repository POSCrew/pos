module.exports = {
  darkmode: true,
  content: [
    "./src/**/*.{html,js,svelte,ts}",
    "./node_modules/ui-commons/**/*.{svelte,ts}",
  ],
  theme: {
    fontFamily: {
      label: ['"Segoeui"', "ui-sans-serif", "system-ui"],
    },

    extend: {
      backgroundImage: {
        "diag-pattern": "url('/diag-pattern.webp')",
      },
      backgroundSize: {
        "400p": "400px",
      },
      colors: {
        primary: "#3323ee55",
      },
    },
  },
  plugins: [],
};
