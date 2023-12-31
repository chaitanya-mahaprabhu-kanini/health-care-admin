import "./Home.css";
import {Link} from 'react-router-dom';

const Home = (props) => {
  return (
    <>
      <div id="wallpaper" class = "homeGrid">
        <div id="side" className ="frosted">
          <div id="introduction">
            <h1>Welcome To</h1>
            <h1>Healthify</h1>
            <h4 style={{ marginTop: "1rem" }}>
              Connect To Doctors. Anywhere. Anytime.
            </h4>
          </div>

          <div id="stripes">
            <div></div>
            <div></div>
            <div></div>
          </div>
        </div>

        <div id="signUp" className ="frosted">
          <h1><Link to = "/Choices">Join Us</Link></h1>
        </div>
        <div id="logIn" className ="frosted">
          <h1><Link to = "/Login">Step In</Link></h1>
        </div>
      </div>
    </>
  );
};

export { Home };
