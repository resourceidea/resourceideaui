import styled from 'styled-components'
import { variant } from 'styled-system'
import { Text } from "@chakra-ui/core";

const StyledText = styled(Text)(
  {
    appearance: 'none',
    margin: '0',
    fontFamily: `"HK Grotesk", san-serif !important`,
  },
  variant({
    variants: {
      heading6: {
        fontSize: '1rem',
        fontWeight: '600'
      },
      subtitle1: {
        fontSize: '0.75rem',
        fontWeight: '600'
      },
    }
  })
)

export default StyledText