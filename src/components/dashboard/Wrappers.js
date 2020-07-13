import styled from "styled-components";
import { Box } from '@chakra-ui/core'

export const TopHeaderRow = styled(Box)`
&>div:nth-child(1) {
    border-right: 1px solid ${props => props.theme.colors.primary[2]};
  }
`;

export const BottomHeaderRow = styled(Box)`
  &>div:nth-child(1) {
    border-right: 1px solid ${props => props.theme.colors.primary[2]};
  }
`;
export const StickyCell = styled(Box)`
  border-right: 1px solid ${props => props.theme.colors.primary[2]};
`;

export const OtherCell = styled(Box)`
  position: relative;
  &:before {
    content: "";
    position: absolute;
    top: 0;
    left: 0;
    width: 22px;
    height: 50px;
    background-color: ${props => props.theme.colors.primary[1]};
    opacity: 0.5;
  }
`;

export const TableWrapper = styled(Box)`
  .table {
    .tr {
      :last-child {
        .td {
          border-bottom: 0;
        }
      }
    }
    .th,
    .td {
      background-color: #fff;
      overflow: hidden;
      :last-child {
        border-right: 0;
      }
      .resizer {
        display: inline-block;
        width: 5px;
        height: 100%;
        position: absolute;
        right: 0;
        top: 0;
        transform: translateX(50%);
        z-index: 1;
        &.isResizing {
          background: red;
        }
      }
    }
    &.sticky {
      overflow: scroll;
      -ms-overflow-style: none;
      scrollbar-width: none;
      &:-webkit-scrollbar {
        display: none;
      }
      .header,
      .footer {
        position: sticky;
        z-index: 1;
        width: fit-content;
      }
      .header {
        top: 0;
      }
      .footer {
        bottom: 0;
      }
      .body {
        position: relative;
        z-index: 0;
      }
      [data-sticky-td] {
        position: sticky;
      }
      [data-sticky-last-left-td] {
      }
      [data-sticky-first-right-td] {
      }
    }
  }
`;