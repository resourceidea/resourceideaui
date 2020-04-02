import React, { Component } from "react";
import { Redirect } from "react-router-dom";
import { Button, Card, CardBody, CardGroup, Col, Container, Form, Input, InputGroup, InputGroupAddon, InputGroupText, Row } from 'reactstrap'; 
class Reset extends Component {  
  constructor() {  
      super();  

      this.state = {
        redirect: false,  
          Email: ''  
      }  
 
      this.Email = this.Email.bind(this);  
  }  
  setRedirect = () => {
    this.setState({
      redirect: true
    })
  }
  renderRedirect = () => {
    if (this.state.redirect) {
      return <Redirect to='/login' />
    }
  }
  Email(event) {  
      this.setState({ Email: event.target.value })  
  }  
  
  
  render() {  


  return (
    <div className="Reset flex-row align-items-center">
                <Container>  
                    <Row className="justify-content-center">  
                        <Col md="9" lg="7" xl="6"> 
                        <CardGroup>  
                                <Card className="p-2">  
                                    <CardBody>
                                    <Form>  
                                            <div class="row" className="mb-2 pageheading">  
                                                <div class="col-sm-12 btn btn-info">  
                                                    Reset Password 
                             </div> 
                             </div>  
                                            <InputGroup className="mb-3">  
  
                                                <Input type="text" onChange={this.Email} placeholder="Enter Email" />  
                                            </InputGroup>  
                                            {this.renderRedirect()}
                                            <Button onClick={this.setRedirect} color="success" block>Send Code</Button>  
                                           
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
export default Reset; 
