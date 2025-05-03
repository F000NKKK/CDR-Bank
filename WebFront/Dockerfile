FROM php:8.2-apache

RUN apt-get update && apt-get install -y \
    libpng-dev \
    libjpeg-dev \
    libfreetype6-dev \
    && docker-php-ext-configure gd --with-freetype --with-jpeg \
    && docker-php-ext-install gd mysqli calendar\
    && docker-php-ext-enable calendar \
    && apt-get clean

RUN a2enmod rewrite

COPY ./apache.conf /etc/apache2/sites-available/000-default.conf
