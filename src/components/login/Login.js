import React, { Component } from "react";
import { Link,Redirect } from "react-router-dom";
import { Button, Card, CardBody, CardGroup, Col, Container, Form, Input, InputGroup, Row } from 'reactstrap'; 
class Login extends Component {  
  constructor() {  
      super();  

      this.state = {  
          Email: '',
          redirect: false,   
          Password: ''  
      }  

      this.Password = this.Password.bind(this);  
      this.Email = this.Email.bind(this);  
  }  

  Email(event) {  
      this.setState({ Email: event.target.value })  
  }  
  Password(event) {  
      this.setState({ Password: event.target.value })  
  } 
  setRedirect = () => {
    this.setState({
      redirect: true
    })
  }
  renderRedirect = () => {
    if (this.state.redirect) {
      return <Redirect to='/' />
    }
  }   

  render() {  


  return (
    <div className="Login flex-row align-items-center">
                <Container>  
                    <Row className="justify-content-center">  
                        <Col md="9" lg="7" xl="6"> 
                        <CardGroup>  
                                <Card className="p-2">  
                                    <CardBody>
                                    <Form>  
                                            <div class="row" className="mb-2 pageheading">  
                                                <div class="col-sm-12 btn btn-primary">  
                                                    Login  
                             </div> 
                             </div>  
                                            <InputGroup className="mb-3">  
  
                                                <Input type="text" onChange={this.Email} placeholder="Enter Email" />  
                                            </InputGroup>  
                                            <InputGroup className="mb-4">  
  
                                                <Input type="password" onChange={this.Password} placeholder="Enter Password" />  
                                            </InputGroup>
                                            
                                            {this.renderRedirect()}
                                            <Button onClick={this.setRedirect} color="success" block>Login</Button>  
                                            

                                            <Link to="/login/reset" className="nav-link">Forgot password?</Link>
                                        </Form>  
                                    </CardBody>  
                                </Card>  
                            </CardGroup>  
                        </Col>  
                    </Row>  
                </Container>  
            </div>  
        );  
    }  
} 
export default Login; 
