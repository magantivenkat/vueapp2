import axios from "axios";

const getTemplates = () => {
  return new Promise((resolve, reject) => {
    axios.get('/api/emails/listemails', {
    })
    .then(function (response) {
      resolve(response.data);
    })
    .catch(function (error) {
      reject(error);
    }) 
  });
};

const getCreateTemplateModel = async (emailType) => {
  return new Promise((resolve, reject) => {
    axios.get('/api/emails/create', {
      params: {
        emailType
      }
    })
    .then(function (response) {
      resolve(response.data);
    })
    .catch(function (error) {
      reject(error);
    }) 
  });
};

const getEditTemplateModel = (id, isLayout) => {
  return new Promise((resolve, reject) => {
    const url = isLayout ? "/api/emails/editlayout" : "/api/emails/edit"
    axios.get(url, {
      params: {
        id
      }
    })
    .then(function (response) {
      resolve(response.data);
    })
    .catch(function (error) {
      reject(error);
    }) 
  });
};

const postEmailModel = async (model, isLayout) => {
  return new Promise((resolve, reject) => {
    const url = isLayout ? "/api/emails/postEmailLayout" : "/api/emails/postEmail"
    
    axios.post(url, model)
    .then(function (response) {
      resolve(response.data);
    })
    .catch(function (error) {
      reject(error);
    }) 
  });
};

const generatePreviewEmails = async (model)=> {
  return new Promise((resolve, reject) => {
    axios.post('/api/emails/preview', model)
    .then(function (response) {
      resolve(response.data);
    })
    .catch(function (error) {
      reject(error);
    }) 
  });
}

const getPreviewEmails = (id)=> {
  return new Promise((resolve, reject) => {
    axios.get('/api/emails/getPreviews', {
      params: {
        id
      }
    })
    .then(function (response) {
      resolve(response.data);
    })
    .catch(function (error) {
      reject(error);
    }) 
  });
}

const sendPreviewEmails = (id)=> {
  return new Promise((resolve, reject) => {
    axios.get('/api/emails/sendBatch', {
      params: {
        id
      }
    })
    .then(function (response) {
      resolve(response.data);
    })
    .catch(function (error) {
      reject(error);
    }) 
  });
}

const getSendEmailModel = (id)=> {
  return new Promise((resolve, reject) => {
    axios.get('/api/emails/send', {
      params: {
        id
      }
    })
    .then(function (response) {
      resolve(response.data);
    })
    .catch(function (error) {
      reject(error);
    }) 
  });
}

export {
  getTemplates,
  getCreateTemplateModel,
  getEditTemplateModel,
  postEmailModel,
  generatePreviewEmails,
  getPreviewEmails,
  sendPreviewEmails,
  getSendEmailModel
};
