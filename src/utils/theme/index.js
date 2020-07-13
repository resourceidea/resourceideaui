import { theme } from "@chakra-ui/core";
import * as colors from './colors'
import icons from './icons'

export const customTheme = {
  ...theme,
  breakpoints: ["30em", "48em", "62em", "80em"],
  fonts: {
    heading: '"HK Grotesk", sans-serif',
    body: "system-ui, sans-serif",
    mono: "Menlo, monospace",
  },
  fontSizes: {
    xs: "0.75rem",
    sm: "0.875rem",
    md: "1rem",
    lg: "1.125rem",
    xl: "1.25rem",
    "2xl": "1.5rem",
    "3xl": "1.875rem",
    "4xl": "2.25rem",
    "5xl": "3rem",
    "6xl": "4rem",
  },
  colors: {
    ...colors,
    primary: colors.navy,
    neutral: colors.neutral,
    text: colors.grey,
  },
  icons
};
