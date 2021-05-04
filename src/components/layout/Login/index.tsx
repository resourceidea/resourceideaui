import React from 'react';
import styled from 'styled-components';
import Text from '@/common/Text'
import { PasswordIcon, EmailIcon } from '@/common/icons'
import Button from '@/common/Button'
import Link from 'next/link'

const Wrapper = styled.div`
`;

const Nav = styled.div`
  padding: 22px 120px;
  margin-bottom: 110px;
`;

const Content = styled.div`
  padding-left: 120px;
  padding-right: 120px;
  display: flex;
  width: 100%;
`;

const Form = styled.form`
  width: 50%;
  .title {
    margin-bottom: 32px;
  }
  ${Button} {
    width: 390px;
  }
  .sign-up {
    margin-top: 22px;
    display: flex;
    font-size: 14px;
    line-height: 21px;
    a {
      color: ${props => props.theme.colors.primary[5]};
      margin-left: 8px;
      font-size: 14px;
      font-weight: 500;
      line-height: 21px;
      &:hover {
        filter: brightness(0.7)
      }
    }
  }
  .forgot-link {
    display: inline-block;
    margin-top: 14px;
    margin-left: auto;
    color: ${props => props.theme.colors.primary[5]};
    font-weight: 500;
    font-size: 14px;
    line-height: 21px;
    &:hover {
      filter: brightness(0.7)
    }
  }
`;
const Preview = styled.div`
  width: 50%;
  img {
    width: 100%;
  }
`;
const InputGroup = styled.div`
  width: 390px;
  position: relative;
  margin-bottom: 22px;
  display: flex;
  flex-direction: column;
  label {
    display: block;
    font-style: normal;
    font-weight: 600;
    font-size: 14px;
    line-height: 21px;
    margin-bottom: 7px;
  }
  svg {
    position: absolute;

    transform: translateY(30px) translateY(-50%) translateX(20px)
  }
  input {
    padding: 16px 20px 16px 50px;
    display: block;
    width: 100%;
    border: 1px solid ${props => props.theme.colors.primary[7]};
    outline: none;
    background-color: white;
    box-sizing: border-box;
    border-radius: 4px;
    font-family: 'Poppins', sans-serif;
    font-style: normal;
    font-weight: normal;
    font-size: 14px;
    line-height: 21px;
  }
`;


const LoginPage: React.FC = () => {
  return (
    <Wrapper>
      <Nav>
        <img src="/images/logo.svg" alt="logo" />
      </Nav>
      <Content>
        <Form action="post" onSubmit={e => e.preventDefault()}>
          <Text variant="h1" className="title">Log in to your account</Text>
          <InputGroup>
            <label htmlFor="email">Email</label>
            <div className="input-wrapper">
              <EmailIcon size={20} />
              <input type="email" name="email" id="email" placeholder="Enter email address" />
            </div>
          </InputGroup>
          <InputGroup>
            <label htmlFor="password">Password</label>
            <div className="input-wrapper">
              <PasswordIcon />
              <input type="password" name="password" id="password" placeholder="Enter password" />
            </div>
            <Link href="/forgot-password" ><a className="forgot-link"><Text variant="caption">Forgot password?</Text></a></Link>
          </InputGroup>
          <Button variant="primary" type="submit">login</Button>
          <div className="sign-up">
            <Text variant="caption">Don’t have an account yet?</Text>
            <Link href="/signup"><a><Text variant="caption">Sign up</Text></a></Link>
          </div>
        </Form>
        <Preview className="preview-img">
          <img src="/images/login-img.png" alt="login-preview" />
        </Preview>
      </Content>
    </Wrapper>
  )
}

export default LoginPage
