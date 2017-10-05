﻿import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { ConfigService } from './services/config.service';
import { DataService } from './services/data.service';
import { HomeComponent } from './home/home.component';
import { CommentsComponent } from './comments/comments.component';
import { routing } from './app.routes';

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        routing
    ],
    declarations: [
        AppComponent,
        CommentsComponent,
        HomeComponent,
    ],
    bootstrap: [AppComponent],
    providers: [
        ConfigService,
        DataService
    ]
})
export class AppModule { }