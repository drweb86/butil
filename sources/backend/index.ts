import express, { Express, Request, Response } from 'express';
var path = require('path');
const app: Express = express();
const port = 3000;

console.log(__dirname);
const frontendArtifacts = path.join(__dirname, '../../rat/build');
app.use(express.static(frontendArtifacts));

app.get('/', function(req, res) {
  res.sendFile(path.join(frontendArtifacts, 'index.html'));
});

app.get('/api', (req: Request, res: Response) => {
    res.send('Hello World!')
  });
  

app.listen(port, () => {
    console.log(`Example app listening on port ${port}`)
  });