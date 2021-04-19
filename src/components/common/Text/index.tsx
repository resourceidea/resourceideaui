import React from "react";
import styled from "styled-components";
import { variant } from "styled-system";

const StyledText = styled("p")(
  {
    fontFamily: "DM Sans, , sans-serif",
    color: "neutral.black1",
  },
  variant({
    variants: {
      h1: {
        fontWeight: "800",
        fontSize: "24px",
        lineHeight: "32px",
      },
      h2: {
        fontWeight: "600",
        fontSize: "24px",
        lineHeight: "32px",
      },
      h3: {
        fontWeight: "500",
        fontSize: "24px",
        lineHeight: "32px",
      },
      h5: {
        fontWeight: "bold",
        fontSize: "16px",
        lineHeight: "24px",
      },
      h6: {
        fontWeight: "normal",
        fontSize: "14px",
        lineHeight: "20px",
      },
      body1: {
        fontWeight: "normal",
        fontSize: "16px",
        lineHeight: "24px",
      },
      body2: {
        fontWeight: "normal",
        fontSize: "14px",
        lineHeight: "22px",
      },
      button: {
        textTransform: "uppercase",
        fontWeight: "600",
        fontSize: "14px",
        lineHeight: "24px",
      },
    },
  })
);

function Text(props) {
  let tags = {
    h1: "h1",
    h2: "h2",
    h3: "h3",
    h4: "h4",
    h5: "h5",
    h6: "h6",
    body1: "p",
    body2: "p",
    caption: "p",
    button: "p",
  };
  let tag = tags[props.variant] || "p";
  return <StyledText as={tag} {...props} />;
}

export default Text;
