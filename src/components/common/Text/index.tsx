import React from "react";
import styled from "styled-components";
import { variant as systemVariant } from "styled-system";

const StyledText = styled('p')(
  {
    fontFamily: "DM Sans, sans-serif",
    margin: 0,
    color: "neutral.black1",
  },
  systemVariant({
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

interface TagTypes {
  h1: "h1"
  h2: "h2"
  h3: "h3"
  h4: "h4"
  h5: "h5"
  h6: "h6"
  body1: "p"
  body2: "p"
  caption: "p"
  button: "p"
};
interface TextProps {
  variant?: keyof TagTypes;
  className?: string;
  children: React.ReactChild;
}
function Text(props: TextProps): React.ReactElement {
  let { variant = 'body1', children, ...rest } = props
  let tag = tags[variant] as keyof JSX.IntrinsicElements

  return <StyledText as={tag} {...rest}>{children}</StyledText>;
}

export default Text;
