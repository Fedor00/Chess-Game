import React, { useState, useEffect } from "react";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";
function Checkmate({ game }) {
   const [modalIsOpen, setIsOpen] = useState(false);

   useEffect(() => {
      if (game && game.status === 2) {
         setIsOpen(true);
      }
   }, [game]);

   return (
      <div>
         <Modal isOpen={modalIsOpen} toggle={() => setIsOpen(!modalIsOpen)}>
            <ModalHeader toggle={() => setIsOpen(!modalIsOpen)}>
               Game Over
            </ModalHeader>
            <ModalBody>Checkmate!</ModalBody>
            <ModalFooter>
               <Button color="primary" onClick={() => setIsOpen(!modalIsOpen)}>
                  Close
               </Button>
            </ModalFooter>
         </Modal>
      </div>
   );
}

export default Checkmate;
