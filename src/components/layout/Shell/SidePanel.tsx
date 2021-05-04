import React from "react";
import styled from "styled-components";
import Link from "next/link";
import { useRouter } from 'next/router'
import Text from "@/common/Text";
import { LogoIcon, SettingsIcon, AdminsIcon, InfoIcon } from '@/common/icons'
import useTheme from '@/lib/hooks/useTheme'
import { sidebarWidth } from './Wrappers'

const Wrapper = styled.div`
  padding: 18px 0px;
  display: flex;
  flex-direction: column;
  align-items: center;
  width: ${sidebarWidth};
  min-height: 768px;
  height: 100vh;
  width: 48px;
  background-color: #003d47;
  .title-link {
    display: block;
  }
  .panel-link {
    padding: 4px;
  }
  ul {
    margin: 0;
    padding: 0;
    padding-top: 14px;
    margin-left: 4px;
    flex-grow: 1;
    display: flex;
    flex-direction: column;
    align-items: center;
    list-style: none;
    li {
      margin-top: 12px;
      &.align-bottom {
       margin-top: auto; 
      }
    }
  }
`;

const SidePanel = () => {
  const theme = useTheme()
  let fill = theme.colors.primary[4]
  return (
    <Wrapper>
      <Link href="/"><a className="title-link"><img src="/icons/logo.svg" /></a></Link>
      <ul>
        <li>
          <Link href="#"><a className="panel-link"><SettingsIcon fill={fill} size={20} /></a></Link>
        </li>
        <li>
          <Link href="#"><a className="panel-link"><AdminsIcon fill={fill} size={20} /></a></Link>
        </li>
        <li className="align-bottom">
          <Link href="#"><a className="panel-link"><InfoIcon fill={fill} size={20} /></a></Link>
        </li>
      </ul>
    </Wrapper>
  )
}

export default SidePanel
