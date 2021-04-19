import styled from "styled-components";
import { variant } from "styled-system";

const StyledButton = styled("button")(
  {
    appearance: "none",
    margin: "0",
    border: "none",
    cursor: "pointer",
    fontSize: "0.75rem !important",
    fontWeight: 600,
    padding: "0.625rem 1.5rem !important",
    fontFamily: `"DM Sans", san-serif !important`,
    borderRadius: "4px",
    textTransform: "uppercase",
  },
  variant({
    variants: {
      primary: {
        color: "neutral.1",
        bg: "teal.5",
        "&:hover": {
          bg: "teal.6",
        },
      },
    },
  })
);

export default StyledButton;
